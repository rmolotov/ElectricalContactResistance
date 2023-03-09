using ECR.Gameplay.Logic;
using ECR.Gameplay.UI;
using ECR.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ECR.Meta.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ActorUI heroUI;
        [SerializeField] private Button returnButton;
        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        public void Initialize(GameObject hero)
        {
            SetupButtons();
            SetupHeroUI(hero);
        }

        private void SetupHeroUI(GameObject hero)
        {
            var heroHealth = hero.GetComponent<IHealth>();
            heroUI?.Initialize(heroHealth);
        }

        private void SetupButtons()
        {
            returnButton.onClick.AddListener(() =>
            {
                _stateMachine.Enter<LoadMetaState>();
            });
        }
    }
}