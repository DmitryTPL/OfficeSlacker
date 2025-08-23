using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Models
{
    public class BaseActionsBindingConfig<TActionEnum, TBinding> : ScriptableObject
        where TActionEnum : Enum
        where TBinding : BaseActionsBinding<TActionEnum>
    {
        [SerializeField] private List<TBinding> _bindings;

        public List<TBinding> Bindings => _bindings;
    }
}