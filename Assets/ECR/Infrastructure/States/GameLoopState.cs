using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.States.Interfaces;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IEnemyFactory _enemyFactory;

        public GameLoopState(GameStateMachine gameStateMachine, IEnemyFactory enemyFactory)
        {
            _stateMachine = gameStateMachine;
            _enemyFactory = enemyFactory;
        }

        public void Enter()
        {
            // TODO: nothing?
            
            Debug.Log(typeof(GameLoopState));
        }

        public void Exit()
        {
            // release enemies' assets there instead on exit from LLS coz they can respawn by timer
            _enemyFactory.CleanUp();
        }
    }
}