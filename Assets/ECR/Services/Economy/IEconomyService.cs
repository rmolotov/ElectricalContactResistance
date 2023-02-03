using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECR.Services.Economy
{
    public interface IEconomyService
    {
        int PlayerCurrency { get; set; }
        Dictionary<string, int> GetInventoryItems { get; }
        List<string> GetAvailableItems();
        (bool, int) IsItemObtainedAndCount(string itemKey);
        Task BuyItem(string itemKey);
        
        event Action<bool> OnCompletedBuying;
        event Action<int> OnCurrencyChanged;
    }
}