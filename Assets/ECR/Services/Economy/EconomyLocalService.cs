using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.Services.StaticData;
using UniRx;

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
        public IntReactiveProperty PlayerCurrency { get; set; } = new();


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

        public void Initialize()
        {
            
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
            if (itemPrice > PlayerCurrency.Value)
            {
                // reject purchase
                OnCompletedBuying?.Invoke(false);
            }
            else
            {
                // process purchase
                _persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var currentCount);
                _persistentDataService.Economy.InventoryItems[itemKey] = currentCount + 1;

                PlayerCurrency.Value -= itemPrice;
                OnCompletedBuying?.Invoke(true);
                
                _saveLoadService.SaveEconomy();
            }
            
            return Task.CompletedTask;
        }
    }
}