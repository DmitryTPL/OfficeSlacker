using Gameplay.Models;
using Zenject;

namespace Installers
{
    public class OfficeLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(destroyCancellationToken);

            Container.BindInterfacesTo<OfficeParametersHolder>().AsSingle();
            Container.BindInterfacesTo<OfficeParametersChangeHandler>().AsSingle();

            Container.BindInterfacesTo<SpawnOfficeEventHandler>().AsSingle();
            Container.BindInterfacesTo<SpawnedOfficeEventsHolder>().AsSingle();

            Container.BindInterfacesTo<OfficeActionsHandler>().AsSingle();
        }
    }
}