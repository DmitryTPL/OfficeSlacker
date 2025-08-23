using System.Linq;
using UnityEngine;
using Zenject;

namespace Gameplay.Models
{
    public interface ISpawnEventHandler
    {
    }

    public class SpawnOfficeEventHandler : ISpawnEventHandler, ITickable
    {
        private readonly EventsConfig _eventsConfig;
        private readonly IGameParametersHolder _gameParametersHolder;
        private readonly ISpawnedOfficeEventsHolder _spawnedEventsHolder;

        public SpawnOfficeEventHandler(EventsConfig eventsConfig, IGameParametersHolder gameParametersHolder, ISpawnedOfficeEventsHolder spawnedEventsHolder)
        {
            _eventsConfig = eventsConfig;
            _gameParametersHolder = gameParametersHolder;
            _spawnedEventsHolder = spawnedEventsHolder;
        }

        public void Tick()
        {
            TrySpawnEvent();
        }

        private void TrySpawnEvent()
        {
            var canSpawn = _spawnedEventsHolder.SpawnedEvents.Values.All(e => e.CanBeParallel);

            if (!canSpawn)
            {
                return;
            }

            float randomValue;

            foreach (var eventKeyValue in _eventsConfig.Events)
            {
                if (_spawnedEventsHolder.SpawnedEvents.ContainsKey(eventKeyValue.Key))
                {
                    continue;
                }

                randomValue = Random.value;

                var isFitToParameters = true;

                foreach (var anchorParameterInfo in eventKeyValue.Value.AnchorParameters)
                {
                    var parameterValue = _gameParametersHolder.GetParameter(anchorParameterInfo.ParameterType);

                    if (!anchorParameterInfo.ParameterValueBorders.IsWithinRange(parameterValue))
                    {
                        isFitToParameters = false;
                        break;
                    }
                }

                if (isFitToParameters && randomValue < eventKeyValue.Value.SpawnProbability)
                {
                    _spawnedEventsHolder.SpawnEvent(eventKeyValue.Key, eventKeyValue.Value);
                    return;
                }
            }
        }
    }
}