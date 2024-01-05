namespace BasicInjector
{
    public interface IInstaller
    {
        void InitializeContainer(ContainerBuilder containerBuilder);
    }
}