using System;
using AYellowpaper.SerializedCollections;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class EventsInfoDictionary : SerializedDictionary<OfficeEventType, BaseEventInfo>
    {
    }

    [CreateAssetMenu(fileName = nameof(EventsConfig), menuName = "Configs/Events")]
    public class EventsConfig : ScriptableObject
    {
        [SerializeField] private EventsInfoDictionary _events;

        public EventsInfoDictionary Events => _events;
    }
}