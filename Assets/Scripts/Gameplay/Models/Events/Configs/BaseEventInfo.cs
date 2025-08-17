using System;
using System.Collections.Generic;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public struct EventAnchorParameter
    {
        [SerializeField] private OfficeParameterType _parameterType;
        [SerializeField] private RangeFloat _parameterValueBorders;

        public OfficeParameterType ParameterType => _parameterType;
        public RangeFloat ParameterValueBorders => _parameterValueBorders;
    }
    
    [Serializable]
    public struct BaseEventInfo
    {
        [SerializeField] private float _spawnProbability;
        [SerializeField] private List<EventAnchorParameter> _anchorParameters;
        [SerializeField] private float _duration;
        [SerializeField] private bool _canBeParallel;
        
        public float SpawnProbability => _spawnProbability;
        public List<EventAnchorParameter> AnchorParameters => _anchorParameters;
        public bool CanBeParallel => _canBeParallel;
        public float Duration => _duration;
    }
}