using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class ActionImplementationView : View<ActionImplementationPresenter>
    {
        [SerializeField] private ActionImplementationPresenter.Data _data;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override BasePresenterViewSharedData SharedData => _data;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            Presenter.Launched.Listen(Launched, destroyCancellationToken);
            Presenter.Finished.Listen(Finished, destroyCancellationToken);
        }

        private void Launched()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Finished()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}