using System.Threading.Tasks;
using ECR.Gameplay.Enemy;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Services.StaticData;
using ECR.StaticData;
using UnityEngine;
using Zenject;

namespace ECR.Infrastructure.Factories
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public EnemyFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }
        
        public async Task WarmUp()
        {
            //todo: asset provider: load loot and spawner
            await Task.CompletedTask;
        }

        public void CleanUp()
        {
            _assetProvider.Cleanup();
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
            health.CurrentHP = health.MaxHP;

            var attack = enemy.GetComponent<EnemyAttack>();
            attack.AttackDamage = config.Current;
            attack.Shield = config.Resistance;
            
            return enemy;
        }
    }
}