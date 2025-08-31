using System;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct ActionImplementationInfo
    {
        [SerializeField] private bool _isTimedAction;
        [SerializeField] private float _actionTime;
        [SerializeField] private bool _isUseActivityButtons;

        public bool IsTimedAction => _isTimedAction;
        public float ActionTime => _actionTime;
        public bool IsUseActivityButtons => _isUseActivityButtons;
    }
}