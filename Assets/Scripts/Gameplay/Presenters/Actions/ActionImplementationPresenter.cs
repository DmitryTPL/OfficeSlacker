using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;

namespace Gameplay.Presenters
{
    public class ActionImplementationPresenter : Presenter
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private ActionImplementationType _actionType;

            public ActionImplementationType ActionType => _actionType;
        }

        private readonly AsyncReactiveProperty<Invoker> _launched = new(default);
        private readonly AsyncReactiveProperty<Invoker> _finished = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> Launched => _launched;
        public IReadOnlyAsyncReactiveProperty<Invoker> Finished => _finished;

        public ActionImplementationPresenter() { }

        [Inject]
        public ActionImplementationPresenter(IActionImplementationBroadcast actionImplementationBroadcast)
        {
            actionImplementationBroadcast.ActionLaunched.WithoutCurrent().ForEachAsync(ActionLaunched, DestroyCancellationToken).Forget();
            actionImplementationBroadcast.ActionFinished.WithoutCurrent().ForEachAsync(ActionFinished, DestroyCancellationToken).Forget();
        }

        private void ActionLaunched(ActionImplementationType actionType)
        {
            var data = GetSharedData<Data>();

            if (data.ActionType == actionType)
            {
                _launched.Invoke();
            }
        }

        private void ActionFinished(ActionImplementationType actionType)
        {
            var data = GetSharedData<Data>();

            if (data.ActionType == actionType)
            {
                _finished.Invoke();
            }
        }
    }
}