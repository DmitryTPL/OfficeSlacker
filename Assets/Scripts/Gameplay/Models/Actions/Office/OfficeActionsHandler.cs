using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IOfficeActionsHandler : IActionsHandler<OfficeActionType>
    {
    }

    public class OfficeActionsHandler : BaseActionsHandler<OfficeActionType, OfficeActionsBinding>, IOfficeActionsHandler
    {
        public OfficeActionsHandler(OfficeActionsBindingConfig config)
            : base(config)
        {
        }

        protected override bool CompareActionTypes(OfficeActionType action1, OfficeActionType action2)
        {
            return action1 == action2;
        }
    }
}