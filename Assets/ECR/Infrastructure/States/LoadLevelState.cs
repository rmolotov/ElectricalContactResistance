using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<SceneName>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public async void Enter(SceneName sceneName)
        {
            Debug.Log(typeof(LoadLevelState) + $" for {sceneName}");
            var sceneInstance = await _sceneLoader.Load(sceneName);
            
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            
        }
    }
}