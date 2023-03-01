using System;

namespace ECR.StaticData
{
    [Serializable]
    public class InventoryItemStaticData
    {
        public string ItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxCount { get; set; }
        public int Price { get; set; }
    }
}