using System.Threading.Tasks;
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

        public async void Enter()
        {
            // TODO: show curtain
            await _uiFactory.WarmUp();
            
            var sceneInstance = await _sceneLoader.Load(SceneName.Meta, OnLoaded);
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }

        private async void OnLoaded(SceneName sceneName)
        {
            await InitUIRoot();
            await InitMainMenu();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private async Task InitMainMenu() =>
            await _uiFactory
                .CreateMainMenu()
                .ContinueWith(m => m.Result.Initialize(), TaskScheduler.FromCurrentSynchronizationContext());
    }
}