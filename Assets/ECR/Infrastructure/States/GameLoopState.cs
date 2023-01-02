using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public GameLoopState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
            Debug.Log(typeof(GameLoopState));
        }

        public void Exit()
        {
            
        }
    }
}