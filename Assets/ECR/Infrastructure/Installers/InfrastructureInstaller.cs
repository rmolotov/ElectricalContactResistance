using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States;
using ECR.Services.Economy;
using ECR.Services.Input;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.Services.StaticData;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>().To<AddressableProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            
            BindServices();
            BindFactories();
            
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle(); //GameStateMachine entry point is Initialize()
        }

        private void BindServices()
        {
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle(); // remote, initializable
            Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle(); // possible remote, initializable
            Container.Bind<ISaveLoadService>().To<SaveLoadLocalService>().AsSingle().NonLazy(); // possible remote, initializable
            Container.Bind<IEconomyService>().To<EconomyLocalService>().AsSingle().NonLazy(); // possible remote, initializable
        }

        private void BindFactories()
        {
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}