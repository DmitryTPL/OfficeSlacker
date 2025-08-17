using System;
using UnityEngine;

[Serializable]
public struct RangeFloat
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    public float Min => _min;
    public float Max => _max;

    public bool IsWithinRange(float value, bool includeMin = false, bool includeMax = false)
    {
        var isGreaterThanMin = includeMin ? value >= _min : value > _min;
        var isLowerThanMax = includeMax ? value <= _max : value < _max;

        return isGreaterThanMin && isLowerThanMax;
    }
}