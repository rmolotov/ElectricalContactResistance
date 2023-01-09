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
            /*TODO:
             load player progress from
             Save/Load service (interface: save(), load())
             */
            
            Debug.Log(typeof(LoadProgressState));
            _stateMachine.Enter<LoadLevelState, SceneName>(SceneName.Core);
        }

        public void Exit()
        {
            
        }
    }
}