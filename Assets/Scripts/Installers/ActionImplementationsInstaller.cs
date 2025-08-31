using Gameplay.Models;
using Zenject;

namespace Installers
{
    public class ActionImplementationsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ActionImplementationBroadcast>().AsSingle();
            Container.BindInterfacesTo<TimedActionsHandler>().AsSingle();

            Container.BindInterfacesTo<ActivityButtonsHandler>().AsSingle();
        }
    }
}