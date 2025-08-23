using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class OfficeActionButtonView : ButtonView<OfficeActionButtonPresenter>
    {
        [SerializeField] private OfficeActionButtonPresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}