using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarsFrameView : View<BypassCarsFramePresenter>
    {
        [SerializeField] private BypassCarsFramePresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}