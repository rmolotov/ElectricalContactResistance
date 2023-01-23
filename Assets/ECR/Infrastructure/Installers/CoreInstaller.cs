using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States;
using ECR.Services.Input;
using ECR.Services.StaticData;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>().To<AddressableProvider>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            
            Container.Bind<SceneLoader>().AsSingle();

            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle(); //GameStateMachine entry point is Initialize()
            
            BindFactories();
        }
        
        private void BindFactories()
        {
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}