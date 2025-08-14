using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Presenters;
using MVP;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views
{
    public class SliderParameterView : View<SliderParameterPresenter>
    {
        [SerializeField] private SliderParameterPresenter.Data _data;

        [SerializeField] private Image _slider;
        
        protected override BasePresenterViewSharedData SharedData => _data;

        protected override void PresenterAttached()
        {
            Presenter.ParameterPercentChanged.ForEachAsync(ParameterValueChanged, destroyCancellationToken).Forget();
        }

        private void ParameterValueChanged(float percent)
        {
            _slider.fillAmount = percent;
        }
    }
}