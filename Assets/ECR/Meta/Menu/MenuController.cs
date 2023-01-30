using ECR.Infrastructure.States;
using ECR.StaticData;
using ECR.UI;
using ECR.UI.CustomComponents;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Menu
{
    public class MenuController : MonoBehaviour
    {
        public ToggleGroup stagesTogglesContainer;
        [SerializeField] private Button startStageButton;

        
        [BoxGroup("Windows")][HorizontalGroup("Windows/Horizontal", 0.5f, LabelWidth = 120)]
        
        [VerticalGroup("Windows/Horizontal/Left")] [LabelText("Settings")]
        [SerializeField] private ButtonForPromised settingsButton;
        [VerticalGroup("Windows/Horizontal/Right")] [HideLabel]
        [SerializeField] private WindowBase settingsWindow;
        
        [VerticalGroup("Windows/Horizontal/Left")] [LabelText("Shop")]
        [SerializeField] private ButtonForPromised shopButton;
        [VerticalGroup("Windows/Horizontal/Right")] [HideLabel]
        [SerializeField] private WindowBase shopWindow;


        [CanBeNull] private StageStaticData _selectedStage;

        public void Init(GameStateMachine stateMachine)
        {
            startStageButton.onClick.AddListener(() =>
            {
                //stateMachine.Enter<LoadLevelState, StageStaticData>(_selectedStage); 
                print(_selectedStage?.StageTitle);
            });

            SetupButtons();
        }

        private void SetupButtons()
        {
            settingsButton.onClick.AddListener(() =>
                settingsWindow
                    .InitAndShow(new PlayerSettingsStaticData())
                    .Then(_ => settingsButton.OnPromisedResolve())
            );
            
            shopButton.onClick.AddListener(() =>  
                shopWindow
                    .InitAndShow("shop items data")
                    .Then(_ => shopButton.OnPromisedResolve())
            );
        }

        public void SelectStage([CanBeNull] StageStaticData staticData) => 
            _selectedStage = staticData;
    }
}