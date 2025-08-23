using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Presenters;
using MVP;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views
{
    public abstract class ButtonView<TPresenter> : View<TPresenter>
        where TPresenter : ButtonPresenter, new()
    {
        [SerializeField] private Button _button;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _button.OnClickAsAsyncEnumerable().ForEachAsync(ButtonClicked, destroyCancellationToken).Forget();
        }

        private void ButtonClicked(AsyncUnit _)
        {
            Presenter.Clicked();
        }
    }
}