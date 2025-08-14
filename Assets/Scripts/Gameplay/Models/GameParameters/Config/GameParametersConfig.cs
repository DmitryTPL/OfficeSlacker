using System;
using AYellowpaper.SerializedCollections;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class ParametersChangeInTimeDictionary : SerializedDictionary<GameParameterType, GameParameterChangeInTimeConfigData>
    {
    }

    [Serializable]
    public class BaseParametersInfoDictionary : SerializedDictionary<GameParameterType, BaseParameterInfo>
    {
    }

    [CreateAssetMenu(fileName = nameof(GameParametersConfig), menuName = "Configs/GameParameters")]
    [Serializable]
    public class GameParametersConfig : ScriptableObject
    {
        [SerializeField] private BaseParametersInfoDictionary _baseParametersInfo;
        [SerializeField] private ParametersChangeInTimeDictionary _changingInTimeParameters;

        public BaseParametersInfoDictionary BaseParametersInfo => _baseParametersInfo;
        public ParametersChangeInTimeDictionary ChangingInTimeParameters => _changingInTimeParameters;
    }
}