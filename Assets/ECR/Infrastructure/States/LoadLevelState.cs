using System.Threading.Tasks;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<SceneName>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IHeroFactory _heroFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IHeroFactory heroFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _heroFactory = heroFactory;
        }

        public async void Enter(SceneName sceneName)
        {
            /*TODO:
             show curtain
             clean up factories, then
             warm up factories
             clean up asset provider  
             */
            
            Debug.Log(typeof(LoadLevelState) + $" for {sceneName}");
            var sceneInstance = await _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private async void OnLoaded(SceneName sceneName)
        {
            await InitGameWold();
            await InitUI();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWold()
        {
            GameObject hero = await InitHero();
            SetupCamera(hero);
        }

        private async Task InitUI()
        {
            // init ui
            await Task.CompletedTask;
        }

        private void SetupCamera(GameObject hero)
        {
            //set up camera follow
        }

        private async Task<GameObject> InitHero() => 
            await _heroFactory.Create(Vector3.zero);
    }
}