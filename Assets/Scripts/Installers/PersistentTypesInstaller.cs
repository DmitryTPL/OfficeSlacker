using Gameplay.Models;
using Zenject;

namespace Installers
{
    public class PersistentTypesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameParametersHolder>().AsSingle();
            Container.BindInterfacesTo<GameParametersChangeHandler>().AsSingle();
        }
    }
}