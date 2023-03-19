using System.Threading.Tasks;
using ECR.Gameplay.Hero;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Services.StaticData;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

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
            var hero = Object.Instantiate(prefab, at, Quaternion.identity);
            
            _container.InjectGameObject(hero);

            var health = hero.GetComponent<HeroHealth>();
            health.MaxHP = config.Voltage;
            health.CurrentHP.Value = health.MaxHP;

            var attack = hero.GetComponent<HeroAttack>();
            attack.AttackDamage.Value = config.Current;
            attack.Shield = config.Resistance;

            return Hero = hero;
        }
    }
}