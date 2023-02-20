using System.Threading.Tasks;
using ECR.Gameplay.Board;
using ECR.Gameplay.Logic;
using ECR.StaticData;
using ECR.StaticData.Board;
using UnityEngine;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IStageFactory
    {
        Task WarmUp();
        void CleanUp();

        Task<Board> CreateBoard(BoardTileStaticData[] tilesData);
        Task<EnemySpawner> CreateEnemySpawner(EnemyType enemyType, Vector3 at);
    }
}