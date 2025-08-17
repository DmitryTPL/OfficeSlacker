using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IGameParametersHolder
    {
        float GetParameter(OfficeParameterType parameterType);
        void SetParameterValue(OfficeParameterType parameterType, float newValue);
    }

    public class OfficeParametersHolder : IGameParametersHolder
    {
        private readonly Dictionary<OfficeParameterType, float> _values = new();

        public OfficeParametersHolder(OfficeParametersConfig officeParametersConfig)
        {
            UpdateValuesWithInitialParameters(officeParametersConfig);
        }

        private void UpdateValuesWithInitialParameters(OfficeParametersConfig officeParametersConfig)
        {
            foreach (var parameter in Enum.GetValues(typeof(OfficeParameterType)).Cast<OfficeParameterType>())
            {
                if (!officeParametersConfig.BaseParametersInfo.ContainsKey(parameter))
                {
                    _values[parameter] = 0; 
                    
                    continue;
                }
                
                _values[parameter] = officeParametersConfig.BaseParametersInfo[parameter].InitialValue;
            }
        }

        public float GetParameter(OfficeParameterType parameterType)
        {
            return _values[parameterType];
        }

        public void SetParameterValue(OfficeParameterType parameterType, float newValue)
        {
            _values[parameterType] = newValue;
        }
    }
}