using System;

namespace ECR.StaticData
{
    [Serializable]
    public class InventoryItemStaticData
    {
        public string ItemId;
        public string Title;
        public string Description;
        public int MaxCount;
        public int Price;
    }
}