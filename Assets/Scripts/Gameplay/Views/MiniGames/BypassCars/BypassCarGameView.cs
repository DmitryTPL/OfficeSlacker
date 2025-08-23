using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarGameView : View<BypassCarGamePresenter>
    {
        [SerializeField] private BypassCarGamePresenter.Data _data;

        protected override BasePresenterViewSharedData SharedData => _data;
    }
}