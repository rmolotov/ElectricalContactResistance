using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.Services.StaticData;

namespace ECR.Services.Economy
{
    public class EconomyLocalService : IEconomyService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        private List<string> _availableItems;

        // TODO: refactor to bool/string Promise w/ confirmation window
        public event Action<bool> OnCompletedBuying;
        
        // TODO: refactor with Rx
        public event Action<int> OnCurrencyChanged;

        public int PlayerCurrency
        {
            get => _persistentDataService.Economy.PlayerCurrency;
            set
            {
                _persistentDataService.Economy.PlayerCurrency = value;
                OnCurrencyChanged?.Invoke(value);
                _saveLoadService.SaveEconomy();
            }
        }
        

        public EconomyLocalService(
            IStaticDataService staticDataService,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService
        )
        {
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }
        

        public Dictionary<string, int> GetInventoryItems =>
            _persistentDataService.Economy.InventoryItems;

        public List<string> GetAvailableItems() => 
            _availableItems ??= _staticDataService.GetAllItems.Select(i => i.ItemId).ToList();

        public (bool, int) IsItemObtainedAndCount(string itemKey)
        {
            var obtained = _persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var count);
            return (obtained, count);
        }

        public Task BuyItem(string itemKey)
        {
            var itemPrice = _staticDataService.ForInventoryItem(itemKey).Price;
            if (itemPrice > PlayerCurrency)
            {
                // reject purchase
                OnCompletedBuying?.Invoke(false);
            }
            else
            {
                // process purchase
                _persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var currentCount);
                _persistentDataService.Economy.InventoryItems[itemKey] = currentCount + 1;

                PlayerCurrency -= itemPrice;
                OnCompletedBuying?.Invoke(true);
                
                _saveLoadService.SaveEconomy();
            }
            
            return Task.CompletedTask;
        }
    }
}