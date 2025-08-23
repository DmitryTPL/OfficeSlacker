using Gameplay.Models;
using Gameplay.SharedData;
using Zenject;

namespace Gameplay.Presenters
{
    public class OfficeActionButtonPresenter : BaseActionButtonPresenter<OfficeActionType>
    {
        public OfficeActionButtonPresenter() { }

        [Inject]
        public OfficeActionButtonPresenter(IOfficeActionsHandler actionsHandler)
            : base(actionsHandler)
        {
        }
    }
}