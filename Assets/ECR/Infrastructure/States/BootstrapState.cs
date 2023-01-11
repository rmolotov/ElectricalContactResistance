using ECR.Infrastructure.States.Interfaces;
using ECR.Services.StaticData;
using UnityEngine;

namespace ECR.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine gameStateMachine, IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            /*TODO:
             create factories
             load persistent data
             */
            
            Debug.Log(typeof(BootstrapState));

            _staticDataService.Initialized += () => 
                _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }
    }
}