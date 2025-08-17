using System;
using AYellowpaper.SerializedCollections;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class ParametersChangeInTimeDictionary : SerializedDictionary<OfficeParameterType, GameParameterChangeInTimeConfigData>
    {
    }

    [Serializable]
    public class BaseParametersInfoDictionary : SerializedDictionary<OfficeParameterType, BaseParameterInfo>
    {
    }

    [CreateAssetMenu(fileName = nameof(OfficeParametersConfig), menuName = "Configs/OfficeParameters")]
    [Serializable]
    public class OfficeParametersConfig : ScriptableObject
    {
        [SerializeField] private BaseParametersInfoDictionary _baseParametersInfo;
        [SerializeField] private ParametersChangeInTimeDictionary _changingInTimeParameters;

        public BaseParametersInfoDictionary BaseParametersInfo => _baseParametersInfo;
        public ParametersChangeInTimeDictionary ChangingInTimeParameters => _changingInTimeParameters;
    }
}