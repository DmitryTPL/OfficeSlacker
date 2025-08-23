using System;
using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    [Serializable]
    public class OfficeActionsBinding : BaseActionsBinding<OfficeActionType>
    {
    }

    [CreateAssetMenu(fileName = nameof(OfficeActionsBindingConfig), menuName = "Configs/OfficeActionsBinding")]
    public class OfficeActionsBindingConfig : BaseActionsBindingConfig<OfficeActionType, OfficeActionsBinding>
    {
    }
}