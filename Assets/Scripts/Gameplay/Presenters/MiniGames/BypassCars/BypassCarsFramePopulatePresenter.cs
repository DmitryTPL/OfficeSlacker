using System;
using System.Collections.Generic;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Presenters
{
    public class BypassCarsFramePopulatePresenter : Presenter, IPopulatableFrame
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private RectTransform _leftTrack;
            [SerializeField] private RectTransform _rightTrack;

            public RectTransform LeftTrack => _leftTrack;
            public RectTransform RightTrack => _rightTrack;
        }

        private readonly BypassCarsConfig _config;
        private readonly IBypassCarsObstaclesPool _obstaclesPool;
        private readonly IBypassCarsFrameSharedData _frameSharedData;
        private readonly List<(BypassCarsObstacleType type, GameObject obstacle)> _spawnedObstacles = new();
        private float _obstaclesHeight;
        private RectTransform _rectTransform;
        private BypassCarsTrackType _previousOccupiedTrack;
        private float _maxObstacleHeight;
        private BypassCarsObstacleType _previousObstacle;

        public BypassCarsFrameType FrameType => _frameSharedData.FrameType;

        [Inject]
        public BypassCarsFramePopulatePresenter(BypassCarsConfig config, IBypassCarsObstaclesPool obstaclesPool,
            IBypassCarsSharedData sharedData, IBypassCarsFrameSharedData frameSharedData)
        {
            _config = config;
            _obstaclesPool = obstaclesPool;
            _frameSharedData = frameSharedData;

            sharedData.PopulatableFrames.Add(this);
        }

        public BypassCarsFramePopulatePresenter()
        {
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            var data = GetSharedData<Data>();

            _rectTransform = data.Transform as RectTransform;

            foreach (var obstacleInfo in _config.Obstacles)
            {
                var heightComponent = obstacleInfo.Prefab.GetComponent<IObstacleHeight>();

                if (heightComponent.Height > _maxObstacleHeight)
                {
                    _maxObstacleHeight = heightComponent.Height;
                }
            }
        }

        public void Populate()
        {
            foreach (var spawnedObstacle in _spawnedObstacles)
            {
                _obstaclesPool.Despawn(spawnedObstacle.type, spawnedObstacle.obstacle);
            }

            _spawnedObstacles.Clear();
            _obstaclesHeight = 0;

            while (true)
            {
                SpawnObstacle();

                if (_rectTransform.rect.height < _obstaclesHeight + _maxObstacleHeight + _config.EmptySpaceBetweenObstaclesDifferentLaneHeight)
                {
                    break;
                }
            }
        }

        private void SpawnObstacle()
        {
            var bottom = _rectTransform.rect.min.y;

            ObstacleInfo obstacleInfo = default;

            foreach (var configObstacle in _config.Obstacles)
            {
                var obstacleRandom = Random.value;

                if (obstacleRandom < configObstacle.SpawnProbability)
                {
                    obstacleInfo = configObstacle;
                    break;
                }
            }

            var trackRandom = Random.value;

            var data = GetSharedData<Data>();

            var track = trackRandom <= 0.5 ? BypassCarsTrackType.Left : BypassCarsTrackType.Right;

            if (_obstaclesHeight > 0 && _previousObstacle != BypassCarsObstacleType.Empty)
            {
                _obstaclesHeight += _previousOccupiedTrack == track
                    ? _config.EmptySpaceBetweenObstaclesSameLaneHeight
                    : _config.EmptySpaceBetweenObstaclesDifferentLaneHeight;
            }

            _previousOccupiedTrack = track;
            _previousObstacle = obstacleInfo.ObstacleType;

            var obstacleHorizontalPosition = track == BypassCarsTrackType.Left
                ? data.LeftTrack.transform.localPosition.x + data.LeftTrack.rect.width / 2
                : data.RightTrack.transform.localPosition.x - data.RightTrack.rect.width / 2;

            var obstacleVerticalPosition = bottom + _obstaclesHeight;

            var position = new Vector3(obstacleHorizontalPosition, obstacleVerticalPosition);

            var obstacle = _obstaclesPool.Spawn(obstacleInfo.ObstacleType, position, data.Transform);

            obstacle.transform.localPosition = new Vector3(obstacleHorizontalPosition, obstacleVerticalPosition, 0);

            var obstacleHeight = obstacle.GetComponent<IObstacleHeight>();

            _obstaclesHeight += obstacleHeight.Height;

            _spawnedObstacles.Add((obstacleInfo.ObstacleType, obstacle));
        }
    }
}