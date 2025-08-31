using System;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;

namespace Gameplay.Presenters
{
    [InstallerGenerationInjection(typeof(BypassCarsFrameSharedData))]
    public class BypassCarsFramePresenter : Presenter, IMovableFrame
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private BypassCarsFrameType _frameType;

            public BypassCarsFrameType FrameType => _frameType;
        }

        private Vector3 _initialPosition;
        private Data _data;
        private readonly IBypassCarsFrameSharedData _frameSharedData;

        public BypassCarsFrameType FrameType => GetSharedData<Data>().FrameType;
        public Vector3 Position => GetSharedData<Data>().Transform.position;

        [Inject]
        public BypassCarsFramePresenter(IBypassCarsSharedData sharedData, IBypassCarsFrameSharedData frameSharedData)
        {
            _frameSharedData = frameSharedData;

            sharedData.MovableFrames.Add(this);
        }

        public BypassCarsFramePresenter()
        {
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();

            _frameSharedData.FrameType = _data.FrameType;

            _initialPosition = _data.Transform.position;
        }

        public void Reset()
        {
            _data.Transform.position = _initialPosition;
        }

        public void AddToPosition(Vector3 delta)
        {
            _data.Transform.position += delta;
        }
    }
}