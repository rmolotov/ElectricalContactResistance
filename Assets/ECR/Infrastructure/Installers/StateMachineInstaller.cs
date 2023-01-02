using ECR.Infrastructure.States;
using Zenject;

namespace ECR.Infrastructure.Installers
{
    public class StateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            
            var stateMachine = new GameStateMachine();
            
            Container.Bind<GameStateMachine>()
                .FromInstance(stateMachine)
                .AsSingle();
            
            stateMachine.Enter<BootstrapState>();
        }
    }
}