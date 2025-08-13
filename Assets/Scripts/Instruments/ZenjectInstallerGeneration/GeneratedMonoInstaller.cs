using Zenject;

public class GeneratedMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallPresenters();
        InstallInjectedInPresenterTypes();
    }

    protected virtual void InstallPresenters()
    {
    }

    protected virtual void InstallInjectedInPresenterTypes()
    {
    }
}