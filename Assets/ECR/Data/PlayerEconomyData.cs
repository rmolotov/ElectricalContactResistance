using System;
using System.Collections.Generic;

namespace ECR.Data
{
    [Serializable]
    public class PlayerEconomyData
    {
        public int PlayerCurrency { get; set; }
        public Dictionary<string, int> InventoryItems { get; set; }
    }
}