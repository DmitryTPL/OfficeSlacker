using System;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class BaseActionsBinding<TActionEnum>
        where TActionEnum : Enum
    {
        [SerializeField] private TActionEnum _actionType;
        [SerializeField] private ActionImplementationType _actionImplementation;

        public TActionEnum ActionType => _actionType;
        public ActionImplementationType Implementation => _actionImplementation;
    }
}