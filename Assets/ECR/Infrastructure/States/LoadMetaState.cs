using System.Threading.Tasks;
using CustomExtensions.Tasks;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.SceneManagement;
using ECR.Infrastructure.States.Interfaces;

namespace ECR.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly SceneLoader _sceneLoader;

        public LoadMetaState(GameStateMachine stateMachine, IUIFactory uiFactory, SceneLoader sceneLoader)
        {
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _ = WarmUpAndLoad().ProcessErrors();
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }
        
        private async Task WarmUpAndLoad()
        {
            // TODO: show curtain
            await _uiFactory.WarmUp();
            
            var sceneInstance = await _sceneLoader.Load(SceneName.Meta);
            await InitUIRoot();
            await InitMainMenu();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private async Task InitMainMenu()
        {
            var controller = await _uiFactory.CreateMainMenu();
            await controller.Initialize();
        }
    }
}