using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using CustomExtensions.Functional;
using ECR.Gameplay.Hero;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Services.StaticData;

namespace ECR.Infrastructure.Factories
{
    public class HeroFactory : IHeroFactory
    {
        private const string HeroPrefabId = "HeroPrefab";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        
        [CanBeNull] public GameObject Hero { get; private set; }

        public HeroFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(key: HeroPrefabId);
        }

        public void CleanUp()
        {
            Hero = null;
            _assetProvider.Release(key: HeroPrefabId);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            var config = _staticDataService.ForHero();
            var prefab = await _assetProvider.Load<GameObject>(key: HeroPrefabId);
            var hero = _container.InstantiatePrefab(prefab, at, Quaternion.identity, null);

            hero.GetComponent<HeroHealth>()
                .With(health => health.MaxHP = config.Voltage)
                .With(health => health.CurrentHP.Value = health.MaxHP);
            hero.GetComponent<HeroAttack>()
                .With(attack => attack.AttackDamage.Value = config.Current)
                .With(attack => attack.Shield = config.Resistance);

            return Hero = hero;
        }
    }
}