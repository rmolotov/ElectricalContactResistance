using Sirenix.OdinInspector;
using Zenject;
using ECR.Infrastructure.States;
using ECR.Services.Logging;

namespace ECR.Gameplay.Logic
{
    public class LevelProgressWatcher : SerializedMonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, ILoggingService loggingService)
        {
            _gameStateMachine = gameStateMachine;
            _loggingService = loggingService;
        }

        public void RunLevel()
        {
            _loggingService.LogMessage($"level ran", this);
        }
    }
}