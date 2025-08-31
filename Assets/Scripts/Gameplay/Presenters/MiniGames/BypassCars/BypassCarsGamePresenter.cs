using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Gameplay.Models;
using Gameplay.SharedData;
using MVP;
using Zenject;

namespace Gameplay.Presenters
{
    [InstallerGenerationInjection(typeof(BypassCarsObstaclesPool))]
    [InstallerGenerationInjection(typeof(BypassCarsSharedData))]
    public class BypassCarsGamePresenter : Presenter
    {
        private readonly IBypassCarsSharedData _sharedData;

        [Inject]
        public BypassCarsGamePresenter(IBypassCarsSharedData sharedData, IActionImplementationBroadcast actionImplementationBroadcast)
        {
            _sharedData = sharedData;

            actionImplementationBroadcast.ActionLaunched.WithoutCurrent()
                .Where(a => a == ActionImplementationType.OfficeArcadeGame)
                .ForEachAsync(GameLaunched, DestroyCancellationToken).Forget();
        }

        public BypassCarsGamePresenter()
        {
        }

        private void GameLaunched(ActionImplementationType _)
        {
            foreach (var populatableFrame in _sharedData.PopulatableFrames)
            {
                if (populatableFrame.FrameType == BypassCarsFrameType.Top)
                {
                    populatableFrame.Populate();
                }
            }
        }
    }
}