using System.Collections.Generic;
using System.Linq;
using Gameplay.SharedData;
using UnityEngine;
using Zenject;

namespace Gameplay.Models
{
    public interface IBypassCarsObstaclesPool
    {
        GameObject Spawn(BypassCarsObstacleType obstacleType, Vector3 position, Transform parent);
        void Despawn(BypassCarsObstacleType obstacleType, GameObject gameObject);
    }

    public class BypassCarsObstaclesPool : IBypassCarsObstaclesPool
    {
        private readonly BypassCarsConfig _config;
        private readonly DiContainer _container;
        private readonly Dictionary<BypassCarsObstacleType, Queue<GameObject>> _obstacles = new();
        private readonly Transform _parent;

        public BypassCarsObstaclesPool(BypassCarsConfig config, DiContainer container)
        {
            _config = config;
            _container = container;
            _parent = new GameObject("BypassCarsObstacles").transform;
        }

        public GameObject Spawn(BypassCarsObstacleType obstacleType, Vector3 position, Transform parent)
        {
            if (_obstacles.ContainsKey(obstacleType) && _obstacles[obstacleType].Count > 0)
            {
                var obstacle = _obstacles[obstacleType].Dequeue();

                obstacle.transform.position = position;
                obstacle.transform.SetParent(parent);
                obstacle.SetActive(true);

                return obstacle;
            }

            var prefab = _config.Obstacles.First(o => o.ObstacleType == obstacleType).Prefab;

            var createdObstacle = _container.InstantiatePrefab(prefab, position, Quaternion.identity, parent);

            createdObstacle.transform.localScale = Vector3.one;

            return createdObstacle;
        }

        public void Despawn(BypassCarsObstacleType obstacleType, GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(_parent);

            if (!_obstacles.ContainsKey(obstacleType))
            {
                _obstacles[obstacleType] = new Queue<GameObject>();
            }

            _obstacles[obstacleType].Enqueue(gameObject);
        }
    }
}