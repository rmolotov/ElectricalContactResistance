using System;
using System.Collections.Generic;
using Zenject;
using ECR.Infrastructure.Factories;
using ECR.Infrastructure.States.Interfaces;
using ECR.Services.Logging;

namespace ECR.Infrastructure.States
{
    public class GameStateMachine : IInitializable
    {
        private readonly StateFactory _stateFactory;
        private readonly ILoggingService _logger;

        private Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(StateFactory stateFactory, ILoggingService loggingService)
        {
            _stateFactory = stateFactory;
            _logger = loggingService;
        }

        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)]    = _stateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)] = _stateFactory.CreateState<LoadProgressState>(),
                [typeof(LoadMetaState)]     = _stateFactory.CreateState<LoadMetaState>(),
                [typeof(LoadLevelState)]    = _stateFactory.CreateState<LoadLevelState>(),
                [typeof(GameLoopState)]     = _stateFactory.CreateState<GameLoopState>()
            };
            
            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState =>
            ChangeState<TState>()
                .Enter();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> =>
            ChangeState<TState>()
                .Enter(payload);


        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            var state = GetState<TState>();
            _currentState = state;
            
            _logger.LogMessage($"state changed to {_currentState.GetType().Name}", this);

            return state;
        }
    }
}