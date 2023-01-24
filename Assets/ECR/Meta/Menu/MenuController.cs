using ECR.Infrastructure.States;
using ECR.StaticData;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Menu
{
    public class MenuController : MonoBehaviour
    {
        public ToggleGroup stagesTogglesContainer;
        
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button startStageButton;


        [CanBeNull] private StageStaticData _selectedStage;

        public void Init(GameStateMachine stateMachine)
        {
            startStageButton.onClick.AddListener(() =>
            {
                //stateMachine.Enter<LoadLevelState, StageStaticData>(_selectedStage); 
                print(_selectedStage?.StageTitle);
            });
        }
        
        public void SelectStage([CanBeNull] StageStaticData staticData) => 
            _selectedStage = staticData;
    }
}