using System.Threading.Tasks;
using ECR.Gameplay.Board;
using ECR.Gameplay.Logic;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Services.StaticData;
using ECR.StaticData;
using ECR.StaticData.Board;
using UnityEngine;
using Zenject;

namespace ECR.Infrastructure.Factories
{
    public class StageFactory : IStageFactory
    {
        private const string EnemySpawnerPrefabId = "EnemySpawnerPrefab";
        private const string BoardPrefabId        = "BoardPrefab";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public StageFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(key: EnemySpawnerPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(key: EnemySpawnerPrefabId);
            _assetProvider.Release(key: BoardPrefabId);
        }

        public async Task<Board> CreateBoard(BoardTileStaticData[] tilesData)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: BoardPrefabId);
            var board = Object.Instantiate(prefab).GetComponent<Board>();
            
            _container.Inject(board);
            
            board.InitializeAndBake(tilesData);
            
            return board;
        }

        public async Task<EnemySpawner> CreateEnemySpawner(EnemyType enemyType, Vector3 at)
        {
            var config = _staticDataService.ForEnemy(enemyType);
            var prefab = await _assetProvider.Load<GameObject>(key: EnemySpawnerPrefabId);
            var spawner = Object.Instantiate(prefab, at, Quaternion.identity).GetComponent<EnemySpawner>();
            
            _container.Inject(spawner);
            
            spawner.Initialize(config);
            
            return spawner;
        }     
    }
}