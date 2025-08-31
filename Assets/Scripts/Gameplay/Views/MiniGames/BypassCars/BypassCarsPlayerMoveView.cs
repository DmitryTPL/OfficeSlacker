using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarsPlayerMoveView : View<BypassCarsPlayerMovePresenter>
    {
        [SerializeField] private BypassCarsPlayerMovePresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}