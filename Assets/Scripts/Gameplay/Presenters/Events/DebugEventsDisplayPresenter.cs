using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;

namespace Gameplay.Presenters
{
    [Serializable]
    public struct OfficeEventText
    {
        [SerializeField] private OfficeEventType _officeEvent;
        [SerializeField] private string _text;

        public OfficeEventType OfficeEvent => _officeEvent;
        public string Text => _text;
    }

    public class DebugEventsDisplayPresenter : Presenter
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private List<OfficeEventText> _officeEventTexts;

            public List<OfficeEventText> OfficeEventTexts => _officeEventTexts;
        }

        public IReadOnlyCollection<string> Texts => _texts;

        private Queue<string> _texts;
        private int _textItemsCount;

        private readonly AsyncReactiveProperty<Invoker> _eventsTextChanged = new(default);
        private Data _data;

        public IReadOnlyAsyncReactiveProperty<Invoker> EventsTextChanged => _eventsTextChanged;

        public DebugEventsDisplayPresenter() { }
        
        [Inject]
        public DebugEventsDisplayPresenter(ISpawnedOfficeEventsHolder spawnedEventsHolder)
        {
            spawnedEventsHolder.EventSpawned.WithoutCurrent().ForEachAsync(EventSpawned).Forget();
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();
        }

        public void SetTextsCount(int count)
        {
            _textItemsCount = count;
            _texts = new Queue<string>(_textItemsCount);
        }

        private void EventSpawned(OfficeEventType eventType)
        {
            while (_texts.Count >= _textItemsCount)
            {
                _texts.Dequeue();
            }

            var text = _data.OfficeEventTexts.FirstOrDefault(t => t.OfficeEvent == eventType).Text ?? eventType.ToString();

            _texts.Enqueue(text);

            _eventsTextChanged.Invoke();
        }
    }
}