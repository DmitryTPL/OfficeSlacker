using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IActivityButtonsHandler
    {
        IReadOnlyAsyncReactiveProperty<ActivityButtonType> ActivityButtonClicked { get; }
        IReadOnlyAsyncReactiveProperty<Invoker> ActivityButtonActivated { get; }
        IReadOnlyAsyncReactiveProperty<Invoker> ActivityButtonDeactivated { get; }
        
        void Clicked(ActivityButtonType buttonType);
    }

    public class ActivityButtonsHandler : IActivityButtonsHandler
    {
        private readonly ActionsImplementationConfig _config;
        private readonly IActivityButtonsHandler _activityButtonsHandler;

        private readonly AsyncReactiveProperty<ActivityButtonType> _activityButtonClicked = new(default);
        private readonly AsyncReactiveProperty<Invoker> _activityButtonActivated = new(default);
        private readonly AsyncReactiveProperty<Invoker> _activityButtonDeactivated = new(default);

        public IReadOnlyAsyncReactiveProperty<ActivityButtonType> ActivityButtonClicked => _activityButtonClicked;
        public IReadOnlyAsyncReactiveProperty<Invoker> ActivityButtonActivated => _activityButtonActivated;
        public IReadOnlyAsyncReactiveProperty<Invoker> ActivityButtonDeactivated => _activityButtonDeactivated;

        public ActivityButtonsHandler(IActionImplementationBroadcast actionImplementationBroadcast, CancellationToken destroyToken,
            ActionsImplementationConfig config)
        {
            _config = config;
            
            actionImplementationBroadcast.ActionLaunched.WithoutCurrent().ForEachAsync(ActionLaunched, destroyToken).Forget();
            actionImplementationBroadcast.ActionFinished.WithoutCurrent().ForEachAsync(ActionFinished, destroyToken).Forget();
        }

        private void ActionLaunched(ActionImplementationType actionType)
        {
            if (!_config.ActionsImplementationData.ContainsKey(actionType))
            {
                return;
            }

            if (_config.ActionsImplementationData[actionType].IsUseActivityButtons)
            {
                _activityButtonActivated.Invoke();
            }
        }

        private void ActionFinished(ActionImplementationType actionType)
        {
            if (!_config.ActionsImplementationData.ContainsKey(actionType))
            {
                return;
            }

            if (_config.ActionsImplementationData[actionType].IsUseActivityButtons)
            {
                _activityButtonDeactivated.Invoke();
            }
        }

        public void Clicked(ActivityButtonType buttonType)
        {
            _activityButtonClicked.Value = buttonType;
        }
    }
}