using System;
using System.Linq;

namespace Gameplay.Models
{
    public static class MainCharacterParameterChangeInTimeExtension
    {
        public static int Execute(this GameParameterChangeInTimeType operation, int value1, int value2)
        {
            return operation switch
            {
                GameParameterChangeInTimeType.Addition => value1 + value2,
                GameParameterChangeInTimeType.Subtraction => value1 - value2,
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };
        }
        
        public static float Execute(this GameParameterChangeInTimeType operation, float value1, float value2)
        {
            return operation switch
            {
                GameParameterChangeInTimeType.Addition => value1 + value2,
                GameParameterChangeInTimeType.Subtraction => value1 - value2,
                _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
            };
        }
    }
}