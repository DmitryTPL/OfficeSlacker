using System;
using Cysharp.Threading.Tasks;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using UnityEngine;
using Zenject;

namespace Gameplay.Presenters
{
    public class SliderParameterPresenter : Presenter, ITickable
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private GameParameterType _parameterType;

            public GameParameterType ParameterType => _parameterType;
        }
        
        private readonly IGameParametersHolder _gameParametersHolder;
        private readonly GameParametersConfig _parametersConfig;
        
        private readonly AsyncReactiveProperty<float> _parameterPercentChanged = new(default);

        public IReadOnlyAsyncReactiveProperty<float> ParameterPercentChanged => _parameterPercentChanged;

        private float _lastParameterPercentValue;
        private Data _data;
        private BaseParameterInfo _parameterInfo;

        public SliderParameterPresenter() { }
        
        [Inject]
        public SliderParameterPresenter(IGameParametersHolder gameParametersHolder, GameParametersConfig parametersConfig)
        {
            _gameParametersHolder = gameParametersHolder;
            _parametersConfig = parametersConfig;
        }

        protected override void InitializeData()
        {
            base.InitializeData();

            _data = GetSharedData<Data>();

            _parameterInfo = _parametersConfig.BaseParametersInfo[_data.ParameterType];
        }

        public void Tick()
        {
            var value = _gameParametersHolder.GetParameter(_data.ParameterType);

            var percent = (value - _parameterInfo.MinValue) / (_parameterInfo.MaxValue - _parameterInfo.MinValue);
            
            if (Math.Abs(percent - _lastParameterPercentValue) > float.Epsilon)
            {
                _parameterPercentChanged.Value = percent;

                _lastParameterPercentValue = value;
            }
        }
    }
}