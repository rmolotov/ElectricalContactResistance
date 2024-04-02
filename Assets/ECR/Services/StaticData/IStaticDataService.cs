using System.Collections.Generic;
using ECR.Services.Interfaces;
using ECR.StaticData;

namespace ECR.Services.StaticData
{
    public interface IStaticDataService : IInitializableAsync
    {
        StageStaticData ForStage(string stageKey);
        List<StageStaticData> GetAllStages { get; }
        
        InventoryItemStaticData ForInventoryItem(string itemKey);
        List<InventoryItemStaticData> GetAllItems { get; }
        
        public HeroStaticData ForHero();
        public EnemyStaticData ForEnemy(EnemyType enemyType);
        
        public void ForWindow();
    }
}