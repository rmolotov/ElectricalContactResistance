using UnityEngine;
using Zenject;
using Lofelt.NiceVibrations;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.Haptic;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.Factories;
using ECR.Services.Economy;
using ECR.Services.Input;
using ECR.Services.Logging;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.Services.StaticData;

namespace ECR.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        [SerializeField] private HapticReceiver hapticReceiver;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressableProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<HapticProvider>().AsSingle()
                .WithArguments(hapticReceiver);
            Container.Bind<SceneLoader>().AsSingle();
            
            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            Container.Bind<IInputService>().To<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle().NonLazy(); // remote, initializable
            Container.BindInterfacesAndSelfTo<PersistentDataService>().AsSingle().NonLazy(); // possible remote, initializable
            Container.BindInterfacesAndSelfTo<SaveLoadLocalService>().AsSingle().NonLazy(); // possible remote, initializable
            Container.BindInterfacesAndSelfTo<EconomyLocalService>().AsSingle().NonLazy(); // possible remote, initializable
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IStageFactory>().To<StageFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}