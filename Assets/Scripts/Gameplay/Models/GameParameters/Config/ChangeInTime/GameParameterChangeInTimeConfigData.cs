using System;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct GameParameterChangeInTimeConfigData
    {
        [SerializeField] private GameParameterChangeInTimeType _changeInTime;
        [SerializeField][Min(float.Epsilon)] private float _changeInTimeValue;

        public GameParameterChangeInTimeType ChangeInTime => _changeInTime;
        public float ChangeInTimeValue => _changeInTimeValue;
    }
}