using System.Threading.Tasks;
using ECR.Infrastructure.AssetManagement;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Meta.Menu;
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
        private const string MenuPrefab = "MainMenuPrefab";
        private const string ShopPrefab = "ShopPrefab";
        private const string StageCardPrefab = "StageCardPrefab";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        private Canvas _uiRoot;

        public UIFactory(DiContainer container, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
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


        private async Task<StageCard> CreateStageCard(StageStaticData stageStaticData, MenuController menu)
        {
            var prefab = await _assetProvider.Load<GameObject>(key: StageCardPrefab);
            var sprite = await _assetProvider.Load<Sprite>(key: stageStaticData.StageKey);
            var card = Object.Instantiate(prefab, menu.stagesTogglesContainer.transform).GetComponent<StageCard>();
           
            card.OnSelect += menu.SelectStage;
            card.Initialize(stageStaticData, sprite, menu.stagesTogglesContainer);

            return card;
        }
    }
}