using System.Threading.Tasks;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Meta.Hud;
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
        private const string UIRootPrefabId       = "UIRootPrefab";
        private const string HudPrefabId          = "HudPrefab";
        private const string MenuPrefabId         = "MainMenuPrefab";
        private const string ShopPrefabId         = "ShopWindowPrefab";
        private const string StageCardPrefabId    = "StageCardPrefab";
        private const string ShopItemCardPrefabId = "ShopItemCardPrefab";

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
            await _assetProvider.Load<GameObject>(key: UIRootPrefabId);
            await _assetProvider.Load<GameObject>(key: HudPrefabId);
            
            await _assetProvider.Load<GameObject>(key: MenuPrefabId);
            await _assetProvider.Load<GameObject>(key: ShopPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(key: MenuPrefabId);
            _assetProvider.Release(key: ShopPrefabId);
            _assetProvider.Release(key: StageCardPrefabId);
            _assetProvider.Release(key: ShopItemCardPrefabId);
        }

        public async Task CreateUIRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<HudController> CreateHud()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: HudPrefabId);
            var hud = Object
                .Instantiate(prefab, _uiRoot.transform)
                .GetComponent<HudController>();

            _container.Inject(hud);
            return hud;
        }

        public async Task<MenuController> CreateMainMenu()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: MenuPrefabId);
            var menu = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<MenuController>();

            foreach (var stageData in _staticDataService.GetAllStages) 
                await CreateStageCard(stageData, menu);

            _container.InjectGameObject(menu.gameObject);
            return menu;
        }

        public async Task<ShopWindow> CreateShop()
        {
            var prefab = await _assetProvider.Load<GameObject>(key: ShopPrefabId);
            var shop = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<ShopWindow>();
            
            foreach (var itemData in _staticDataService.GetAllItems)
                await CreateShopItemCard(itemData, shop.itemsCardsContainer);

            _container.Inject(shop);
            return shop;
        }

        private async Task<StageCard> CreateStageCard(StageStaticData stageStaticData, MenuController menu)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: StageCardPrefabId);
            var sprite = await _assetProvider.Load<Sprite>(key: stageStaticData.StageKey);
            var card = Object.Instantiate(prefab, menu.stagesTogglesContainer.transform).GetComponent<StageCard>();

            card.OnSelect += st => menu.SelectedStage.Value = st;
            card.Initialize(stageStaticData, sprite, menu.stagesTogglesContainer);

            return card;
        }

        private async Task<ShopItemCard> CreateShopItemCard(InventoryItemStaticData shopItemStaticData, RectTransform container)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: ShopItemCardPrefabId);
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