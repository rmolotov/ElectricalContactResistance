using ECR.Infrastructure.Factories.Interfaces;
using ECR.StaticData;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Logic
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemyStaticData _enemyStaticData;

        private IEnemyFactory _enemyFactory;

        [Inject]
        private void Construct(IEnemyFactory enemyFactory) =>
            _enemyFactory = enemyFactory;

        public void Initialize(EnemyStaticData enemyStaticData)
        {
            _enemyStaticData = enemyStaticData;
            Spawn();
        }


        private async void Spawn()
        {
            //todo: use config arg instead of enemy type
            var enemy = await _enemyFactory.Create(_enemyStaticData.EnemyType, transform);
        }
    }
}