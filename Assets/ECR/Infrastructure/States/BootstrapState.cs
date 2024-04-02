using System.Collections.Generic;
using ECR.Infrastructure.States.Interfaces;
using ECR.Services.Interfaces;

namespace ECR.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly List<IInitializableAsync> _initializableServices;

        public BootstrapState(GameStateMachine gameStateMachine, List<IInitializableAsync> initializableServices)
        {
            _stateMachine = gameStateMachine;
            _initializableServices = initializableServices;
        }

        public async void Enter()
        {
            foreach (var service in _initializableServices) 
                await service.InitializeAsync();
            
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }
    }
}