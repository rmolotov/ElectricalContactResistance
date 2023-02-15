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
        private readonly ISpawnersFactory _spawnersFactory;

        private StageStaticData _pendingStageStaticData;

        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            IUIFactory uiFactory,
            IHeroFactory heroFactory,
            ISpawnersFactory spawnersFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _heroFactory = heroFactory;
            _spawnersFactory = spawnersFactory;
        }

        public async void Enter(StageStaticData stageStaticData)
        {
            _pendingStageStaticData = stageStaticData;
            /*TODO:
             show curtain
             clean-up/warm-up enemyFactory?
             */
            
            _heroFactory.CleanUp();
            _spawnersFactory.CleanUp();

            await _heroFactory.WarmUp();
            await _spawnersFactory.WarmUp();

            Debug.Log(typeof(LoadLevelState) + $" for {stageStaticData.StageKey}");
            var sceneInstance = await _sceneLoader.Load(SceneName.Core, OnLoaded);
        }

        public void Exit()
        {
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
            GameObject hero = await InitHero();
            SetupCamera(hero);
            
            SetupBoardTiles();
            SetupEnemySpawners();
        }

        private void SetupBoardTiles()
        {
            // setup stage board tiles based on pendingStage 
            // bake runtime navmesh?
        }

        private void SetupEnemySpawners()
        {
            foreach (var spawnerStaticData in _pendingStageStaticData.EnemySpawners)
            {
                _spawnersFactory.CreateEnemySpawner(spawnerStaticData.EnemyType, spawnerStaticData.Position);
            }
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
            await _heroFactory.Create(Vector3.zero);
    }
}