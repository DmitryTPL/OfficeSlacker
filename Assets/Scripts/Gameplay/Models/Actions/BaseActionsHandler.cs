using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.SharedData;
using UnityEngine;
using Zenject;

namespace Gameplay.Models
{
    public interface IActionsHandler<in TActionEnum>
        where TActionEnum : Enum
    {
        
        void Launch(TActionEnum actionType);
    }

    public abstract class BaseActionsHandler<TActionEnum, TBinding> : IActionsHandler<TActionEnum>
        where TActionEnum : Enum
        where TBinding : BaseActionsBinding<TActionEnum>
    {
        private readonly BaseActionsBindingConfig<TActionEnum, TBinding> _config;
        private IActionImplementationBroadcast _actionImplementationBroadcast;

        protected BaseActionsHandler(BaseActionsBindingConfig<TActionEnum, TBinding> config)
        {
            _config = config;
        }

        [Inject]
        private void AddDependencies(IActionImplementationBroadcast actionImplementationBroadcast)
        {
            _actionImplementationBroadcast = actionImplementationBroadcast;
        }

        public void Launch(TActionEnum actionType)
        {
            var binding = _config.Bindings.FirstOrDefault(b => CompareActionTypes(b.ActionType,actionType));

            if (binding == null)
            {
                Debug.Log($"Can't find binding in config for action {actionType}");
                return;
            }

            _actionImplementationBroadcast.LaunchAction(binding.Implementation);
        }

        protected abstract bool CompareActionTypes(TActionEnum action1, TActionEnum action2);
    }
}