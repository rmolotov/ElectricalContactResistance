using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
            Debug.Log(typeof(BootstrapState));
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }
    }
}