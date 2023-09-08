using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.States.Interfaces;
using ECR.Services.LevelProgress;

namespace ECR.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IHeroFactory _heroFactory;
        private readonly ILevelProgressService _levelProgressService;

        public GameLoopState(GameStateMachine gameStateMachine, IHeroFactory heroFactory, IEnemyFactory enemyFactory,
            ILevelProgressService levelProgressService)
        {
            _stateMachine = gameStateMachine;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _levelProgressService = levelProgressService;
            _levelProgressService = levelProgressService;
        }

        public void Enter()
        {
            _levelProgressService.LevelProgressWatcher.RunLevel();
        }

        public void Exit()
        {
            // release enemies' assets there instead on exit from LLS coz they can respawn by timer
            _enemyFactory.CleanUp();
            _heroFactory.CleanUp();
        }
    }
}