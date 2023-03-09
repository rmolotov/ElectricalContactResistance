using System;
using System.Threading.Tasks;
using ECR.Gameplay.Enemy;
using ECR.Gameplay.UI;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Services.StaticData;
using ECR.StaticData;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ECR.Infrastructure.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IHeroFactory _heroFactory;

        public EnemyFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService,
            IHeroFactory heroFactory)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _heroFactory = heroFactory;
        }
        
        public async Task WarmUp()
        {
            foreach (var enemyType in (EnemyType[]) Enum.GetValues(typeof(EnemyType)))
                await _assetProvider.Load<GameObject>(key: enemyType.ToString());
        }

        public void CleanUp()
        {
            foreach (var enemyType in (EnemyType[]) Enum.GetValues(typeof(EnemyType)))
                _assetProvider.Release(key: enemyType.ToString());
        }
        
        public async Task<GameObject> Create(EnemyType enemyType, Transform parent)
        {
            // TODO: use configKey instead prefab data
            var config = _staticDataService.ForEnemy(enemyType);
            
            var prefab = await _assetProvider.Load<GameObject>(key: config.EnemyType.ToString());
            var enemy = Object.Instantiate(prefab, parent.position, parent.rotation, parent);

            _container.InjectGameObject(enemy);

            var health = enemy.GetComponent<EnemyHealth>();
            health.MaxHP = config.Capacity;
            health.CurrentHP.Value = health.MaxHP;

            var actorUi = enemy.GetComponentInChildren<ActorUI>();
            actorUi?.Initialize(health);

            var attack = enemy.GetComponent<EnemyAttack>();
            attack.AttackDamage.Value = config.Current;
            attack.Shield = config.Resistance;
            
            return enemy;
        }
    }
}