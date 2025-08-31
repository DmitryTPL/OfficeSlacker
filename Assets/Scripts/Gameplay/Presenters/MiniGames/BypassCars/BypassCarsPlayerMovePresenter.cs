using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Presenters
{
    public class BypassCarsPlayerMovePresenter : Presenter
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private Transform _leftLane;
            [SerializeField] private Transform _rightLane;

            public Transform LeftLane => _leftLane;
            public Transform RightLane => _rightLane;
        }

        private Data _data;

        public BypassCarsPlayerMovePresenter() { }

        [Inject]
        public BypassCarsPlayerMovePresenter(IActivityButtonsHandler activityButtonsHandler)
        {
            activityButtonsHandler.ActivityButtonClicked
                .WithoutCurrent()
                .Where(b => b == ActivityButtonType.Left)
                .ForEachAsync(MoveLeft, DestroyCancellationToken).Forget();

            activityButtonsHandler.ActivityButtonClicked
                .WithoutCurrent()
                .Where(b => b == ActivityButtonType.Right)
                .ForEachAsync(MoveRight, DestroyCancellationToken).Forget();
        }


        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();

            _data.Transform.position = Random.value >= 0.5 ? _data.RightLane.position : _data.LeftLane.position;
        }

        private void MoveLeft(ActivityButtonType _)
        {
            _data.Transform.position = _data.LeftLane.position;
        }

        private void MoveRight(ActivityButtonType _)
        {
            _data.Transform.position = _data.RightLane.position;
        }
    }
}