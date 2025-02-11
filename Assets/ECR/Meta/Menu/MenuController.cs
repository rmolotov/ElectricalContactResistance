using System;
using System.Threading.Tasks;
using CustomExtensions.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using ECR.Infrastructure.Factories.Interfaces;
using ECR.Infrastructure.States;
using ECR.Meta.Shop;
using ECR.Services.Logging;
using ECR.Services.PersistentData;
using ECR.Services.SaveLoad;
using ECR.StaticData;
using ECR.UI.CustomComponents;
using ECR.UI.Windows;
using Sirenix.OdinInspector;

namespace ECR.Meta.Menu
{
    public class MenuController : MonoBehaviour
    {
        public readonly ReactiveProperty<StageStaticData> SelectedStage = new();
        public ToggleGroup stagesTogglesContainer;
        [SerializeField] private Button startStageButton;


        [BoxGroup("Windows")]
        [HorizontalGroup("Windows/Horizontal", 0.5f, LabelWidth = 120)]
        
        [VerticalGroup("Windows/Horizontal/Left")]
        [LabelText("Settings")] [SerializeField] private ButtonForPromised settingsButton;

        [VerticalGroup("Windows/Horizontal/Right")]
        [HideLabel] [SerializeField] private WindowBase settingsWindow;

        [VerticalGroup("Windows/Horizontal/Left")] 
        [LabelText("Shop")] [SerializeField] private ButtonForPromised shopButton;

        private GameStateMachine _stateMachine;
        private ILoggingService _logger;
        private IUIFactory _uiFactory;
        private IPersistentDataService _persistentDataService;
        private ISaveLoadService _saveLoadService;

        private ShopWindow _shopWindow;

        [Inject]
        private void Construct(
            GameStateMachine stateMachine,
            ILoggingService loggingService,
            IUIFactory uiFactory,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _logger = loggingService;
            _uiFactory = uiFactory;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public async Task Initialize()
        {
            await CreateShop();
            await UnityTaskExtensions.UnitySynchronizationContext;
            
            SetupButtons();
            
            _logger.LogMessage("initialized", this);
        }

        private async Task CreateShop() => 
            _shopWindow = await _uiFactory.CreateShop();

        private void SetupButtons()
        {
            SelectedStage
                .Throttle(TimeSpan.FromTicks(1))
                .Subscribe(st => startStageButton.interactable = st != null);
            
            startStageButton.onClick.AddListener(() =>
            {
                _stateMachine.Enter<LoadLevelState, StageStaticData>(SelectedStage.Value);
            });

            settingsButton.onClick.AddListener(() =>
                settingsWindow
                    .InitAndShow(_persistentDataService.Settings)
                    .Then(ok =>
                    {
                        settingsButton.OnPromisedResolve();
                        if (ok) _saveLoadService.SaveSettings(); // TODO: else -> _sls.RestoreSavedSettings?
                    })
            );

            shopButton.onClick.AddListener(() =>
                _shopWindow
                    .InitAndShow("shop items data")
                    .Then(_ => shopButton.OnPromisedResolve())
            );
        }
    }
}