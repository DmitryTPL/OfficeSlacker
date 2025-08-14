using AsyncAppTime;
using Zenject;

namespace Gameplay.Models
{
    public interface IMainCharacterParametersChangeHandler
    {
    }

    public class GameParametersChangeHandler : IMainCharacterParametersChangeHandler, ITickable
    {
        private readonly GameParametersConfig _config;
        private readonly IGameParametersHolder _gameParametersHolder;
        private readonly IAppTime _appTime;

        public GameParametersChangeHandler(GameParametersConfig config, IGameParametersHolder gameParametersHolder, IAppTime appTime)
        {
            _config = config;
            _gameParametersHolder = gameParametersHolder;
            _appTime = appTime;
        }

        public void Tick()
        {
            ChangeParameter();
        }

        private void ChangeParameter()
        {
            foreach (var parameter in _config.ChangingInTimeParameters)
            {
                var value = _gameParametersHolder.GetParameter(parameter.Key);

                var result = parameter.Value.ChangeInTime.Execute(value, parameter.Value.ChangeInTimeValue * _appTime.DeltaTime);
                
                _gameParametersHolder.SetParameterValue(parameter.Key, result);
            }
        }
    }
}