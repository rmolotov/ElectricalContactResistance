using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;

        public LoadLevelState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter(string payload)
        {
            Debug.Log(typeof(LoadLevelState) + $" for {payload}");
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            
        }
    }
}