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
            _stateMachine.Enter<LoadLevelState, string>("level name test");
        }

        public void Exit()
        {
            
        }
    }
}