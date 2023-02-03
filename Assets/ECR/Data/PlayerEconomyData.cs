using System;
using System.Collections.Generic;

namespace ECR.Data
{
    [Serializable]
    public class PlayerEconomyData
    {
        public int PlayerCurrency;
        public Dictionary<string, int> InventoryItems;
    }
}