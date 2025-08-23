using System;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct ActionImplementationInfo
    {
        [SerializeField] private bool _isTimedAction;
        [SerializeField] private float _actionTime;

        public bool IsTimedAction => _isTimedAction;
        public float ActionTime => _actionTime;
    }
}