using ECR.Infrastructure.Factories.Interfaces;
using ECR.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Logic
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons]
        private EnemyType enemyType;

        private IEnemyFactory _enemyFactory;

        [Inject]
        private void Construct(IEnemyFactory enemyFactory) =>
            _enemyFactory = enemyFactory;

        private void Start() => 
            Spawn();

        private async void Spawn()
        {
            //todo: use config arg instead of enemy type
            var enemy = await _enemyFactory.Create(enemyType, enemyType.ToString(), transform);
        }
    }
}