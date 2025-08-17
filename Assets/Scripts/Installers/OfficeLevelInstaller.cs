using Gameplay.Models;
using Zenject;

namespace Installers
{
    public class OfficeLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<OfficeParametersHolder>().AsSingle();
            Container.BindInterfacesTo<OfficeParametersChangeHandler>().AsSingle();
            
            Container.BindInterfacesTo<SpawnOfficeEventHandler>().AsSingle();
            Container.BindInterfacesTo<SpawnedOfficeEventsHolder>().AsSingle();
        }
    }
}