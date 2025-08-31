using System;
using System.Collections.Generic;
using System.Linq;
using AsyncAppTime;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;

namespace Gameplay.Presenters
{
    public class BypassCarsFramesMovePresenter : Presenter, ITickable
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private Transform _bottomThreshold;

            public Vector3 FrameBottomThreshold => _bottomThreshold.position;
        }

        private readonly IAppTime _appTime;
        private readonly BypassCarsConfig _config;
        private readonly IBypassCarsSharedData _sharedData;
        private Data _data;
        private float _bottomFrameOffset;
        private bool _isLaunched;
        private readonly Queue<IMovableFrame> _framesArrange = new();
        private Dictionary<BypassCarsFrameType, IMovableFrame> _framesCache = new();

        public BypassCarsFramesMovePresenter() { }

        [Inject]
        public BypassCarsFramesMovePresenter(IAppTime appTime, IActionImplementationBroadcast actionImplementationBroadcast,
            BypassCarsConfig config, IBypassCarsSharedData sharedData)
        {
            _appTime = appTime;
            _config = config;
            _sharedData = sharedData;

            actionImplementationBroadcast.ActionLaunched.WithoutCurrent()
                .Where(a => a == ActionImplementationType.OfficeArcadeGame)
                .ForEachAsync(GameLaunched, DestroyCancellationToken).Forget();

            actionImplementationBroadcast.ActionFinished.WithoutCurrent()
                .Where(a => a == ActionImplementationType.OfficeArcadeGame)
                .ForEachAsync(GameFinished, DestroyCancellationToken).Forget();
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();

            _framesCache = _sharedData.MovableFrames.ToDictionary(f => f.FrameType, f => f);

            _bottomFrameOffset = (_framesCache[BypassCarsFrameType.Top].Position.y - _framesCache[BypassCarsFrameType.Middle].Position.y) * 3;
        }

        private void GameLaunched(ActionImplementationType _)
        {
            _isLaunched = true;

            foreach (var frameKeyValue in _framesCache)
            {
                frameKeyValue.Value.Reset();
            }

            _framesArrange.Clear();

            _framesArrange.Enqueue(_framesCache[BypassCarsFrameType.Bottom]);
            _framesArrange.Enqueue(_framesCache[BypassCarsFrameType.Middle]);
            _framesArrange.Enqueue(_framesCache[BypassCarsFrameType.Top]);
        }

        private void GameFinished(ActionImplementationType _)
        {
            _isLaunched = false;
        }

        public void Tick()
        {
            if (!IsEnable || _data == null || !_isLaunched)
            {
                return;
            }

            MoveFrames();
            RearrangeFrames();
        }

        private void MoveFrames()
        {
            var frameYPositionChange = _appTime.DeltaTime * _config.Speed;

            foreach (var movableFrameKeyValue in _framesCache)
            {
                movableFrameKeyValue.Value.AddToPosition(-new Vector3(0, frameYPositionChange, 0));
            }
        }

        private void RearrangeFrames()
        {
            if (_framesArrange.Peek().Position.y >= _data.FrameBottomThreshold.y)
            {
                return;
            }

            var frame = _framesArrange.Dequeue();

            foreach (var populatableFrame in _sharedData.PopulatableFrames)
            {
                if (populatableFrame.FrameType == frame.FrameType)
                {
                    populatableFrame.Populate();
                }
            }

            frame.AddToPosition(new Vector3(0, _bottomFrameOffset, 0));

            _framesArrange.Enqueue(frame);
        }
    }
}