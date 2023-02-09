using System.Threading.Tasks;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Meta.Menu;
using ECR.Meta.Shop;
using ECR.Services.Economy;
using ECR.Services.StaticData;
using ECR.StaticData;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ECR.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefab = "UIRootPrefab";
        private const string HudPrefab = "HudPrefab";
        private const string MenuPrefab = "MainMenuPrefab";
        private const string ShopPrefab = "ShopWindowPrefab";
        private const string StageCardPrefab = "StageCardPrefab";
        private const string ShopItemCardPrefab = "ShopItemCardPrefab";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IEconomyService _economyService;

        private Canvas _uiRoot;


        public UIFactory(
            DiContainer container, 
            IAssetProvider assetProvider, 
            IStaticDataService staticDataService, 
            IEconomyService economyService
        )
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _economyService = economyService;
        }

        public async Task WarmUp()
        {
            //todo: asset provider: load loot and spawner
            await Task.CompletedTask;
        }

        public void CleanUp()
        {
            _assetProvider.Cleanup();
        }

        public async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: UIRootPrefab);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<GameObject> CreateHud()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: HudPrefab);
            var hud = Object.Instantiate(prefab, _uiRoot.transform);

            return hud;
        }

        public async Task<MenuController> CreateMainMenu()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: MenuPrefab);
            var menu = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<MenuController>();

            foreach (var stageData in _staticDataService.GetAllStages)
            {
                await CreateStageCard(stageData, menu);
            }
            
            _container.Inject(menu);
            return menu;
        }

        public async Task<ShopWindow> CreateShop()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: ShopPrefab);
            var shop = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<ShopWindow>();
            
            foreach (var itemData in _staticDataService.GetAllItems)
            {
                await CreateShopItemCard(itemData, shop.itemsCardsContainer);
            }
            
            _container.Inject(shop);
            return shop;
        }

        private async Task<StageCard> CreateStageCard(StageStaticData stageStaticData, MenuController menu)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: StageCardPrefab);
            var sprite = await _assetProvider.Load<Sprite>(key: stageStaticData.StageKey);
            var card = Object.Instantiate(prefab, menu.stagesTogglesContainer.transform).GetComponent<StageCard>();
           
            card.OnSelect += menu.SelectStage;
            card.Initialize(stageStaticData, sprite, menu.stagesTogglesContainer);

            return card;
        }

        private async Task<ShopItemCard> CreateShopItemCard(InventoryItemStaticData shopItemStaticData, RectTransform container)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: ShopItemCardPrefab);
            var sprite = await _assetProvider.Load<Sprite>(key: shopItemStaticData.ItemId);
            var card = Object.Instantiate(prefab, container).GetComponent<ShopItemCard>();
            var obtainedCount = _economyService.IsItemObtainedAndCount(shopItemStaticData.ItemId).Item2;
            
            card.OnBuyClick += arg =>
            {
                _economyService.BuyItem(arg);
                var updatedCount = _economyService.IsItemObtainedAndCount(arg).Item2;
                card.UpdateObtainedCount(updatedCount);
            };
            card.Initialize(shopItemStaticData, sprite, obtainedCount);

            return card;
        }
    }
}