using Zenject;

namespace AsyncAppTime
{
    public class AppTimeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InitExecutionOrder();

            Container.BindInterfacesAndSelfTo<AppTime>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppTimeApplierPresenter>().AsSingle();
            
            Container.Bind<AppTimeApplierView>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void InitExecutionOrder()
        {
            Container.BindExecutionOrder<AppTimeApplierPresenter>(-50);
        }
    }
}