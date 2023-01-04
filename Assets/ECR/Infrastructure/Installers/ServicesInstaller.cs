using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var assetProvider = new AddressableProvider();
            var sceneLoader = new SceneLoader(assetProvider);
            var stateMachine = new GameStateMachine(sceneLoader); // entry point is Initialize()

            Container.BindInterfacesAndSelfTo<AddressableProvider>().FromInstance(assetProvider).AsSingle().NonLazy();
            Container.Bind<SceneLoader>().FromInstance(sceneLoader).AsSingle().NonLazy();
            Container.Bind<IInitializable>().To<GameStateMachine>().FromInstance(stateMachine).AsSingle();
        }
    }
}