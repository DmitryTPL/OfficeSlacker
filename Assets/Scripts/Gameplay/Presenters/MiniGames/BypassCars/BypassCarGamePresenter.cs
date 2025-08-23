using System;
using System.Collections.Generic;
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
    public class BypassCarGamePresenter : Presenter, ITickable
    {
        private readonly IAppTime _appTime;

        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [Header("Frames")]
            [SerializeField] private Transform _upperFrame;
            [SerializeField] private Transform _middleFrame;
            [SerializeField] private Transform _bottomFrame;
            [Header("Settings")]
            [SerializeField] [Min(0)] private float _speed = 1;
            [SerializeField] private Transform _bottomThreshold;

            public Transform UpperFrame => _upperFrame;
            public Transform MiddleFrame => _middleFrame;
            public Transform BottomFrame => _bottomFrame;
            public float Speed => _speed;
            public Vector3 FrameBottomThreshold => _bottomThreshold.position;
        }

        private List<Vector3> _initializePositions = new();
        private Data _data;
        private float _bottomFrameOffset;
        private bool _isLaunched;
        private Queue<Transform> _framesArrange = new();

        public BypassCarGamePresenter() { }

        [Inject]
        public BypassCarGamePresenter(IAppTime appTime, IActionImplementationBroadcast actionImplementationBroadcast)
        {
            _appTime = appTime;

            actionImplementationBroadcast.ActionLaunched.WithoutCurrent()
                .Where(a => a == ActionImplementationType.OfficeArcadeGame)
                .ForEachAsync(GameLaunched, DestroyCancellationToken).Forget();

            actionImplementationBroadcast.ActionFinished.WithoutCurrent()
                .Where(a => a == ActionImplementationType.OfficeArcadeGame)
                .ForEachAsync(GameFinished, DestroyCancellationToken).Forget();
        }

        private void GameLaunched(ActionImplementationType _)
        {
            _isLaunched = true;

            _data.UpperFrame.position = _initializePositions[0];
            _data.MiddleFrame.position = _initializePositions[1];
            _data.BottomFrame.position = _initializePositions[2];

            _framesArrange.Clear();

            _framesArrange.Enqueue(_data.BottomFrame);
            _framesArrange.Enqueue(_data.MiddleFrame);
            _framesArrange.Enqueue(_data.UpperFrame);
        }

        private void GameFinished(ActionImplementationType _)
        {
            _isLaunched = false;
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();

            _bottomFrameOffset = (_data.UpperFrame.position.y - _data.MiddleFrame.position.y) * 3;

            _initializePositions = new()
            {
                _data.UpperFrame.position,
                _data.MiddleFrame.position,
                _data.BottomFrame.position
            };
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
            var frameYPositionChange = _appTime.DeltaTime * _data.Speed;

            _data.BottomFrame.position -= new Vector3(0, frameYPositionChange, 0);
            _data.MiddleFrame.position -= new Vector3(0, frameYPositionChange, 0);
            _data.UpperFrame.position -= new Vector3(0, frameYPositionChange, 0);
        }

        private void RearrangeFrames()
        {
            if (_framesArrange.Peek().position.y >= _data.FrameBottomThreshold.y)
            {
                return;
            }

            var frame = _framesArrange.Dequeue();

            frame.position += new Vector3(0, _bottomFrameOffset, 0);

            _framesArrange.Enqueue(frame);
        }
    }
}