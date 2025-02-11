using System.Threading.Tasks;
using UnityEngine;
using CustomExtensions.Tasks;
using ECR.Data;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using ECR.StaticData;

namespace ECR.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<StageStaticData>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IHeroFactory _heroFactory;
        private readonly IStageFactory _stageFactory;

        private StageStaticData _pendingStageStaticData;
        private StageProgressData _stageProgressData;

        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            IUIFactory uiFactory,
            IHeroFactory heroFactory,
            IStageFactory stageFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _heroFactory = heroFactory;
            _stageFactory = stageFactory;
        }

        public void Enter(StageStaticData stageStaticData)
        {
            _pendingStageStaticData = stageStaticData;
            _stageProgressData = new StageProgressData();

            _ = WarmUpAndLoad().ProcessErrors();
        }

        public void Exit()
        {
            _stageFactory.CleanUp();
            _pendingStageStaticData = null;
        }

        private async Task WarmUpAndLoad()
        {
            /* TODO:
             show curtain
             warm-up enemyFactory?
             */
            
            await _heroFactory.WarmUp();
            await _stageFactory.WarmUp();

            var sceneInstance = await _sceneLoader.Load(SceneName.Core);
            await InitUIRoot();
            await InitGameWold();
            await InitUI();
            
            _stateMachine.Enter<GameLoopState>();
        }
        
        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private async Task InitGameWold()
        {
            await SetupBoard();

            _stageProgressData.Hero = await SetupHero();
            SetupCamera(_stageProgressData.Hero);

            await SetupEnemySpawners();
        }

        private async Task InitUI()
        {
            await _uiFactory
                .CreateHud()
                .ContinueWith(
                    m => m.Result.Initialize(_pendingStageStaticData, _stageProgressData),
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SetupBoard() => 
            await _stageFactory.CreateBoard(_pendingStageStaticData.BoardTiles);

        private async Task<GameObject> SetupHero() => 
            await _heroFactory.Create(_pendingStageStaticData.PlayerSpawnPoint);

        private void SetupCamera(GameObject hero)
        {
            //set up camera follow
        }

        private async Task SetupEnemySpawners()
        {
            foreach (var spawnerData in _pendingStageStaticData.EnemySpawners)
            {
                var spawner = await _stageFactory.CreateEnemySpawner(spawnerData.EnemyType, spawnerData.Position);
                _stageProgressData.EnemySpawners.Add(spawner);
            }
        }
    }
}