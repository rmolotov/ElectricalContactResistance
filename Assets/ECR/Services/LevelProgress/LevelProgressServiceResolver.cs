using System;
using Zenject;
using ECR.Gameplay.Logic;

namespace ECR.Services.LevelProgress
{
    public class LevelProgressServiceResolver : IInitializable, IDisposable
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly LevelProgressWatcher _levelProgressWatcher;
        
        public LevelProgressServiceResolver(
            ILevelProgressService levelProgressService,
            [Inject(Source = InjectSources.Local, Optional = true)] LevelProgressWatcher levelProgressWatcher)
        {
            _levelProgressService = levelProgressService;
            _levelProgressWatcher = levelProgressWatcher;
        }
        
        public void Initialize() => 
            _levelProgressService.InitForLevel(_levelProgressWatcher);

        public void Dispose() => 
            _levelProgressService.InitForLevel(null);
    }
}