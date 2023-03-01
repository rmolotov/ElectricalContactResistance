using ECR.Gameplay.Logic;
using UnityEngine;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        // obsolete
        // [SerializeField] private EnemySpawner[] enemySpawners;
        
        public override void InstallBindings()
        {
            // obsolete
            // foreach (var spawner in enemySpawners) Container.QueueForInject(spawner);
        }
    }
}