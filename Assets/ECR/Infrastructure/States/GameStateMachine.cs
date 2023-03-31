using System;
using System.Collections.Generic;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;
using ECR.Services.Economy;
using ECR.Services.Logging;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.Services.StaticData;
using Zenject;

namespace ECR.Infrastructure.States
{
    public class GameStateMachine : IInitializable
    {
        private readonly ILoggingService _logger;
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(
            SceneLoader sceneLoader,
            ILoggingService loggingService,
            IStaticDataService staticDataService,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService,
            IEconomyService economyService,
            IUIFactory uiFactory,
            IHeroFactory heroFactory,
            IStageFactory stageFactory,
            IEnemyFactory enemyFactory
        )
        {
            _logger = loggingService;
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)]    = new BootstrapState(this, staticDataService),
                [typeof(LoadProgressState)] = new LoadProgressState(this, persistentDataService, saveLoadService, economyService),
                [typeof(LoadMetaState)]     = new LoadMetaState(this, uiFactory, sceneLoader),
                [typeof(LoadLevelState)]    = new LoadLevelState(this, sceneLoader, uiFactory, heroFactory, stageFactory),
                [typeof(GameLoopState)]     = new GameLoopState(this, heroFactory, enemyFactory),
            };
        }

        public void Initialize() => 
            Enter<BootstrapState>();

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