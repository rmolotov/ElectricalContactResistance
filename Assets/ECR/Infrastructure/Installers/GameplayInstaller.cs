using ECR.Services.Input;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputService>().To<InputService>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}