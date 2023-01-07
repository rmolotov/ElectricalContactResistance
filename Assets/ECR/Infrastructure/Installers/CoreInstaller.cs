using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States;
using ECR.Services.StaticData;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>().To<AddressableProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle(); //GameStateMachine entry point is Initialize()
        }
    }
}