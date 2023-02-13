using System.Threading.Tasks;
using ECR.StaticData;
using UnityEngine;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IEnemyFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<GameObject> Create(EnemyType enemyType, Transform parent);
    }
}