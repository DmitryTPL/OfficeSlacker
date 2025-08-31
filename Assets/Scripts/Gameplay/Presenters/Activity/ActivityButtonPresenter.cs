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
    public class ActivityButtonPresenter : ButtonPresenter
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private ActivityButtonType _buttonType;

            public ActivityButtonType ButtonType => _buttonType;
        }

        private readonly IActivityButtonsHandler _activityButtonsHandler;

        public ActivityButtonPresenter() { }

        [Inject]
        public ActivityButtonPresenter(IActivityButtonsHandler activityButtonsHandler)
        {
            _activityButtonsHandler = activityButtonsHandler;
            
            activityButtonsHandler.ActivityButtonActivated.Listen(Activate, DestroyCancellationToken);
            activityButtonsHandler.ActivityButtonDeactivated.Listen(Deactivate, DestroyCancellationToken);
        }

        private void Activate()
        {
            var data = GetSharedData<Data>();
            
            data.Transform.gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            var data = GetSharedData<Data>();
            
            data.Transform.gameObject.SetActive(false);
        }

        public override void Clicked()
        {
            var data = GetSharedData<Data>();

            _activityButtonsHandler.Clicked(data.ButtonType);
        }
    }
}