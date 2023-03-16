using System.Linq;
using ECR.Data;
using ECR.Gameplay.Logic;
using ECR.Gameplay.UI;
using ECR.Infrastructure.States;
using ECR.Services.Economy;
using ECR.StaticData;
using ECR.UI.Windows;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ECR.Meta.Hud
{
    public class HudController : MonoBehaviour
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

        private void SetupHeroUI(GameObject hero)
        {
            var heroHealth = hero.GetComponent<IHealth>();
            heroUI.Initialize(heroHealth, false);
        }

        private void SetupButtons()
        {
            returnButton.onClick.AddListener(() =>
            {
                _stateMachine.Enter<LoadMetaState>();
            });
        }

        private void SetupStageWindow(StageStaticData stageStaticData, string text) =>
            stagePopup
                .InitAndShow(text, stageStaticData.StageTitle)
                .Then(toMenu =>
                {
                    if (toMenu) _stateMachine.Enter<LoadMetaState>();
                    else _stateMachine.Enter<LoadLevelState, StageStaticData>(stageStaticData);
                });
    }
}