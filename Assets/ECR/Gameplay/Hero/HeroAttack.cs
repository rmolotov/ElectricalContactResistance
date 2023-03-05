using ECR.Services.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        private IInputService _inputService;

        [SerializeField] private HeroAnimator animator;

        // TODO: redactor fields and logic
        public int Shield;
        public int AttackDamage;

        [Inject]
        private void Construct(IInputService inputService) 
            => _inputService = inputService;

        private void Start() => 
            _inputService.AttackPressed += OnAttack;

        private void OnDisable() => 
            _inputService.AttackPressed -= OnAttack;

        [Button("Attack"), GUIColor(0,0,1)]
        private void OnAttack()
        {
            animator.PlayAttack();
            
            // PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1.0f);
            // for (int i = 0; i < Hit(); ++i)
            // {
            //     _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            // }
        }
    }
}