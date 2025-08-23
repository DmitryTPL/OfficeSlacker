using System.Collections.Generic;
using AsyncAppTime;
using Cysharp.Threading.Tasks;
using Gameplay.SharedData;
using Zenject;

namespace Gameplay.Models
{
    public interface IActionImplementationBroadcast
    {
        IReadOnlyAsyncReactiveProperty<ActionImplementationType> ActionLaunched { get; }
        IReadOnlyAsyncReactiveProperty<ActionImplementationType> ActionFinished { get; }

        void LaunchAction(ActionImplementationType actionImplementation);
        void FinishAction(ActionImplementationType actionImplementation);
    }

    public class ActionImplementationBroadcast : IActionImplementationBroadcast
    {
        private readonly ActionsImplementationConfig _config;
        private readonly IAppTime _appTime;

        private readonly AsyncReactiveProperty<ActionImplementationType> _actionLaunched = new(default);
        private readonly AsyncReactiveProperty<ActionImplementationType> _actionFinished = new(default);

        public IReadOnlyAsyncReactiveProperty<ActionImplementationType> ActionLaunched => _actionLaunched;
        public IReadOnlyAsyncReactiveProperty<ActionImplementationType> ActionFinished => _actionFinished;

        public void LaunchAction(ActionImplementationType actionImplementation)
        {

            _actionLaunched.Value = actionImplementation;
        }

        public void FinishAction(ActionImplementationType actionImplementation)
        {

            _actionFinished.Value = actionImplementation;
        }
    }

    public struct LaunchedActionInfo
    {
        public float TimeLaunched { get; }

        public LaunchedActionInfo(float timeLaunched)
        {
            TimeLaunched = timeLaunched;
        }
    }
}