using System;
using System.Collections.Generic;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct ObstacleInfo
    {
        [SerializeField] private BypassCarsObstacleType _obstacleType;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _spawnProbability;

        public BypassCarsObstacleType ObstacleType => _obstacleType;
        public GameObject Prefab => _prefab;
        public float SpawnProbability => _spawnProbability;
    }

    [CreateAssetMenu(fileName = nameof(BypassCarsConfig), menuName = "Configs/BypassCars")]
    public class BypassCarsConfig : ScriptableObject
    {
        [SerializeField] private List<ObstacleInfo> _obstacles;
        [SerializeField] [Min(float.Epsilon)] private float _speed = 1;
        [SerializeField] private float _emptySpaceBetweenObstaclesSameLaneHeight;
        [SerializeField] private float _emptySpaceBetweenObstaclesDifferentLaneHeight;

        public List<ObstacleInfo> Obstacles => _obstacles;
        public float Speed => _speed;
        public float EmptySpaceBetweenObstaclesSameLaneHeight => _emptySpaceBetweenObstaclesSameLaneHeight;
        public float EmptySpaceBetweenObstaclesDifferentLaneHeight => _emptySpaceBetweenObstaclesDifferentLaneHeight;
    }
}