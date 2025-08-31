using System;
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
        [SerializeField] private float _nextClickDelay = 0.1f;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            _button.OnClickAsAsyncEnumerable().ForEachAwaitAsync(ButtonClicked, destroyCancellationToken).Forget();
        }

        private async UniTask ButtonClicked(AsyncUnit _)
        {
            Presenter.Clicked();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_nextClickDelay));
        }
    }
}