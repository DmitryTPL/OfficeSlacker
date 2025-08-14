using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IGameParametersHolder
    {
        float GetParameter(GameParameterType parameterType);
        void SetParameterValue(GameParameterType parameterType, float newValue);
    }

    public class GameParametersHolder : IGameParametersHolder
    {
        private readonly Dictionary<GameParameterType, float> _values = new();

        public GameParametersHolder(GameParametersConfig gameParametersConfig)
        {
            foreach (var parameter in Enum.GetValues(typeof(GameParameterType)).Cast<GameParameterType>())
            {
                _values[parameter] = gameParametersConfig.BaseParametersInfo[parameter].InitialValue;
            }
        }

        public float GetParameter(GameParameterType parameterType)
        {
            return _values[parameterType];
        }

        public void SetParameterValue(GameParameterType parameterType, float newValue)
        {
            _values[parameterType] = newValue;
        }
    }
}