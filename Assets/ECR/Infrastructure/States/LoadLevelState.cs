using System.Threading.Tasks;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using ECR.StaticData;
using UnityEngine;

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

        public async void Enter(StageStaticData stageStaticData)
        {
            _pendingStageStaticData = stageStaticData;
            
            /* TODO:
             show curtain
             warm-up enemyFactory?
             */
            
            await _heroFactory.WarmUp();
            await _stageFactory.WarmUp();

            Debug.Log(typeof(LoadLevelState) + $" for {stageStaticData.StageKey}");
            var sceneInstance = await _sceneLoader.Load(SceneName.Core, OnLoaded);
        }

        public void Exit()
        {
            _heroFactory.CleanUp();
            _stageFactory.CleanUp();
            _pendingStageStaticData = null;
        }

        private async void OnLoaded(SceneName sceneName)
        {
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
            await SetupBoardTiles();
            await SetupEnemySpawners();
            
            GameObject hero = await InitHero();
            SetupCamera(hero);
            
            // TODO: bake runtime navmesh?
        }

        private async Task SetupBoardTiles() => 
            await _stageFactory.CreateBoard(_pendingStageStaticData.BoardTiles);

        private async Task SetupEnemySpawners()
        {
            foreach (var spawnerStaticData in _pendingStageStaticData.EnemySpawners)
                await _stageFactory.CreateEnemySpawner(spawnerStaticData.EnemyType, spawnerStaticData.Position);
        }

        private async Task InitUI()
        {
            await _uiFactory
                .CreateHud()
                .ContinueWith(m => m.Result.Initialize(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetupCamera(GameObject hero)
        {
            //set up camera follow
        }

        private async Task<GameObject> InitHero() => 
            await _heroFactory.Create(_pendingStageStaticData.PlayerSpawnPoint);
    }
}