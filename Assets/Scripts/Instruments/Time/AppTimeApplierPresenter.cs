using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using MVP;
using UnityEngine;
using Zenject;

namespace AsyncAppTime
{
    public class AppTimeApplierPresenter : Presenter, ITickable, IFixedTickable
    {
        private float _timeScaleToChange = -1;
        private float _previousTimeScale;

        private readonly IAppTime _appTime;

        public AppTimeApplierPresenter()
        {
        }

        [Inject]
        public AppTimeApplierPresenter(IAppTime appTime)
        {
            _appTime = appTime;
            _previousTimeScale = Time.timeScale;
            
            _appTime.TimeScale.WithoutCurrent().ForEachAsync(OnTimeScaleChanged, DestroyCancellationToken).Forget();
        }

        public void Tick()
        {
            _appTime.Time = Time.time;
            _appTime.DeltaTime = Time.deltaTime;

            if (_timeScaleToChange >= 0)
            {
                Time.timeScale = _timeScaleToChange;
                _timeScaleToChange = -1;
            }

            if (Math.Abs(_previousTimeScale - Time.timeScale) > float.Epsilon)
            {
                _appTime.TimeScale.Value = Time.timeScale;
            }

            _previousTimeScale = Time.timeScale;
        }

        public void FixedTick()
        {
            _appTime.FixedDeltaTime = Time.fixedDeltaTime;
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            _timeScaleToChange = timeScale;
        }
    }
}