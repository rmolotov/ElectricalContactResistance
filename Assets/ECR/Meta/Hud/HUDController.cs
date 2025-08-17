using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using UniRx.Triggers;
using CustomExtensions.Tasks;
using ECR.Data;
using ECR.Infrastructure.States;
using ECR.Services.Economy;
using ECR.Gameplay.Logic;
using ECR.Gameplay.UI;
using ECR.StaticData;
using ECR.UI.Windows;

namespace ECR.Meta.HUD
{
    public class HUDController : MonoBehaviour
    {
        private const string LoseText = "You've lost and should start stage again.";
        private const string WinText =  "You've won and got some bucks.";
        
        [SerializeField] private ActorUI heroUI;
        [SerializeField] private Button returnButton;
        [SerializeField] private TwoButtonWindow stagePopup;
        
        private GameStateMachine _stateMachine;
        private IEconomyService _economyService;

        [Inject]
        private void Construct(GameStateMachine stateMachine, IEconomyService economyService)
        {
            _stateMachine = stateMachine;
            _economyService = economyService;
        }

        public void Initialize(StageStaticData stageStaticData, StageProgressData stageProgressData)
        {
            SetupButtons();
            SetupHeroUI(stageProgressData.Hero);
            SetupStageProgressReactions(stageStaticData, stageProgressData);
        }

        private void SetupStageProgressReactions(StageStaticData stageStaticData, StageProgressData stageProgressData)
        {
            stageProgressData.EnemySpawners
                .Select(sp => sp.enemiesRemainder)
                .Zip()
                .Where(r => r.All(rm => rm == 0))
                .Subscribe(_ =>
                {
                    _economyService.PlayerCurrency.Value += 100;
                    SetupStageWindow(stageStaticData, WinText);
                });
            
            stageProgressData.Hero
                .OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    SetupStageWindow(stageStaticData, LoseText);
                });
        }

        private void SetupButtons() =>
            returnButton.onClick.AddListener(() => 
                _stateMachine.Enter<LoadMetaState>());

        private void SetupHeroUI(GameObject hero) => 
            heroUI.Initialize(hero.GetComponent<IHealth>(), skipInitAnim: false);

        private void SetupStageWindow(StageStaticData stageStaticData, string text) =>
            stagePopup
                .InitAndShow(text, stageStaticData.StageTitle).Task
                .ContinueWithUnitySynchronizationContext(task =>
                {
                    if (task.Result) _stateMachine.Enter<LoadMetaState>();
                    else _stateMachine.Enter<LoadLevelState, StageStaticData>(stageStaticData);
                });
    }
}