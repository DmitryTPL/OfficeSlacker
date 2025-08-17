using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface ISpawnedOfficeEventsHolder
    {
        IReadOnlyAsyncReactiveProperty<OfficeEventType> EventSpawned { get; }
        IReadOnlyDictionary<OfficeEventType, SpawnedEventInfo> SpawnedEvents { get; }

        void SpawnEvent(OfficeEventType eventType, BaseEventInfo value);
    }

    public class SpawnedOfficeEventsHolder : ISpawnedOfficeEventsHolder
    {
        private readonly Dictionary<OfficeEventType, SpawnedEventInfo> _spawnedEvents = new();

        private readonly AsyncReactiveProperty<OfficeEventType> _eventSpawned = new(default);

        public IReadOnlyAsyncReactiveProperty<OfficeEventType> EventSpawned => _eventSpawned;

        public IReadOnlyDictionary<OfficeEventType, SpawnedEventInfo> SpawnedEvents => _spawnedEvents;

        public void SpawnEvent(OfficeEventType eventType, BaseEventInfo eventInfo)
        {
            _spawnedEvents[eventType] = new SpawnedEventInfo(eventInfo.Duration, eventInfo.CanBeParallel);

            _eventSpawned.Value = eventType;
        }
    }
}