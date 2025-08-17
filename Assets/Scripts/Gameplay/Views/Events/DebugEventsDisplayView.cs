using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Presenters;
using MVP;
using TMPro;
using UnityEngine;

namespace Gameplay.Views
{
    public class DebugEventsDisplayView : View<DebugEventsDisplayPresenter>
    {
        [SerializeField] private DebugEventsDisplayPresenter.Data _data;
        [SerializeField] private TMP_Text[] _texts;

        protected override BasePresenterViewSharedData SharedData => _data;

        protected override void PresenterAttached()
        {
            base.PresenterAttached();

            Presenter.SetTextsCount(_texts.Length);

            Presenter.EventsTextChanged.Listen(EventsTextChanged, destroyCancellationToken);
        }

        private void EventsTextChanged()
        {
            var eventTexts = Presenter.Texts;

            var i = 0;

            foreach (var eventText in eventTexts)
            {
                _texts[i].text = eventText;

                i++;
            }
        }
    }
}