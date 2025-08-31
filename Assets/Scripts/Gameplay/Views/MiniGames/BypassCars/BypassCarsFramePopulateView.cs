using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarsFramePopulateView : View<BypassCarsFramePopulatePresenter>
    {
        [SerializeField] private BypassCarsFramePopulatePresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}