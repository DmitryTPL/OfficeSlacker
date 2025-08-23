using System.Collections.Generic;
using System.Threading;
using AsyncAppTime;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.SharedData;
using UnityEngine;
using Zenject;

namespace Gameplay.Models
{
    public interface ITimedActionsHandler
    {
        float ActionTimeLeft(ActionImplementationType actionType);
    }

    public class TimedActionsHandler : ITimedActionsHandler, ITickable
    {
        private readonly IActionImplementationBroadcast _actionImplementationBroadcast;
        private readonly ActionsImplementationConfig _config;
        private readonly IAppTime _appTime;

        private Dictionary<ActionImplementationType, LaunchedActionInfo> _launchedActions = new();

        public TimedActionsHandler(IActionImplementationBroadcast actionImplementationBroadcast, ActionsImplementationConfig config, IAppTime appTime,
            CancellationToken destroyToken)
        {
            _actionImplementationBroadcast = actionImplementationBroadcast;
            _config = config;
            _appTime = appTime;

            actionImplementationBroadcast.ActionLaunched.WithoutCurrent().ForEachAsync(ActionLaunched, destroyToken).Forget();
        }

        private void ActionLaunched(ActionImplementationType actionType)
        {
            if (!_config.ActionsImplementationData.ContainsKey(actionType))
            {
                return;
            }

            _launchedActions[actionType] = new LaunchedActionInfo(_appTime.Time);
        }

        private void FinishAction(ActionImplementationType actionType)
        {
            _launchedActions.Remove(actionType);

            _actionImplementationBroadcast.FinishAction(actionType);
        }

        public void Tick()
        {
            var removeActions = new List<ActionImplementationType>();

            foreach (var action in _launchedActions)
            {
                if (_config.ActionsImplementationData[action.Key].IsTimedAction
                    && _appTime.Time > action.Value.TimeLaunched + _config.ActionsImplementationData[action.Key].ActionTime)
                {
                    removeActions.Add(action.Key);
                }
            }

            foreach (var action in removeActions)
            {
                FinishAction(action);
            }
        }

        public float ActionTimeLeft(ActionImplementationType actionType)
        {
            if (!_launchedActions.ContainsKey(actionType))
            {
                Debug.Log($"Action {actionType} not launched");
                return 0;
            }

            return _config.ActionsImplementationData[actionType].ActionTime - (_appTime.Time - _launchedActions[actionType].TimeLaunched);
        }
    }
}