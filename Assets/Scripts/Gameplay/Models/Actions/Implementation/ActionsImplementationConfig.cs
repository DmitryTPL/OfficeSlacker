using System;
using AYellowpaper.SerializedCollections;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class ActionImplementationInfoDictionary : SerializedDictionary<ActionImplementationType, ActionImplementationInfo>
    {
    }

    [CreateAssetMenu(fileName = nameof(ActionsImplementationConfig), menuName = "Configs/ActionsImplementation")]
    public class ActionsImplementationConfig : ScriptableObject
    {
        [SerializeField] private ActionImplementationInfoDictionary _actionsImplementationData;

        public ActionImplementationInfoDictionary ActionsImplementationData => _actionsImplementationData;
    }
}