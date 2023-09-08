using UnityEngine;
using Zenject;
using ECR.Gameplay.Logic;

namespace ECR.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelProgressWatcher levelProgressWatcher;
        
        public override void InstallBindings()
        {
            Container.BindInstance(levelProgressWatcher);
        }
    }
}