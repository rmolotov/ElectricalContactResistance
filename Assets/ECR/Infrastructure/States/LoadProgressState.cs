using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
            Debug.Log(typeof(LoadProgressState));
            _stateMachine.Enter<LoadLevelState, SceneName>(SceneName.Meta);
        }

        public void Exit()
        {
            
        }
    }
}