using ECR.Gameplay.Logic;

namespace ECR.Services.LevelProgress
{
    public interface ILevelProgressService
    {
        LevelProgressWatcher LevelProgressWatcher { get; set; }
        
        void InitForLevel(LevelProgressWatcher levelController);
    }
}