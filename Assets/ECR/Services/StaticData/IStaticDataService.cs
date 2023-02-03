using System;
using System.Collections.Generic;
using ECR.StaticData;
using Zenject;

namespace ECR.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        public Action Initialized { get; set; }
        StageStaticData ForStage(string stageKey);
        List<StageStaticData> GetAllStages { get; }
        InventoryItemStaticData ForInventoryItem(string itemKey);
        List<InventoryItemStaticData> GetAllItems { get; }
        public HeroStaticData ForHero();
        public EnemyStaticData ForEnemy(EnemyType enemyType);
        public void ForWindow();
    }
}