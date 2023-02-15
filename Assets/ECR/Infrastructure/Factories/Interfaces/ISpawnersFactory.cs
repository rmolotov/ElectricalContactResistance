using System.Threading.Tasks;
using ECR.Gameplay.Logic;
using ECR.StaticData;
using UnityEngine;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface ISpawnersFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<EnemySpawner> CreateEnemySpawner(EnemyType enemyType, Vector3 at);
    }
}