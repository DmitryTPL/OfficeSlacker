using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class ActivityButtonView : ButtonView<ActivityButtonPresenter>
    {
        [SerializeField] private ActivityButtonPresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}