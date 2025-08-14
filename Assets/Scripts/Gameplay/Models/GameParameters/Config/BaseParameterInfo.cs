using System;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct BaseParameterInfo
    {
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _initialValue;

        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public float InitialValue => _initialValue;
    }
}