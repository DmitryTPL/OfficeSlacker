using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarsFramesMoveView : View<BypassCarsFramesMovePresenter>
    {
        [SerializeField] private BypassCarsFramesMovePresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}