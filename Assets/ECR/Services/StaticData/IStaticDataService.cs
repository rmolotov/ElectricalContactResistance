using ECR.StaticData;
using Zenject;

namespace ECR.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        public HeroStaticData ForHero();
        public EnemyStaticData ForEnemy(EnemyType enemyType);
        public void ForLevel();
        public void ForWindow();
    }
}