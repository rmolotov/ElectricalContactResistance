using Zenject;

namespace ECR.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        public void ForHero();
        public void ForEnemy();
        public void ForLevel();
        public void ForWindow();
    }
}