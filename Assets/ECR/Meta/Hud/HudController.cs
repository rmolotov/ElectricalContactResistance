using ECR.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ECR.Meta.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Button returnButton;
        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async void Initialize()
        {
            SetupButtons();
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