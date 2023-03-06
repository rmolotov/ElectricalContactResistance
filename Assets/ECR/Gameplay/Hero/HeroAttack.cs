using ECR.Services.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;

        // TODO: redactor fields and logic
        public int Shield;
        
        [ShowInInspector]
        public int AttackDamage
        {
            get => _attackDamage;
            set
            {
                _attackDamage = value;
                var emission = attackVFX.emission;
                emission.rateOverTimeMultiplier = AttackDamage;
            }
        }

        private IInputService _inputService;
        private int _attackDamage;

        [Inject]
        private void Construct(IInputService inputService) 
            => _inputService = inputService;

        private void Start() =>
            _inputService.AttackPressed += OnAttack;

        private void OnDestroy() =>
            _inputService.AttackPressed -= OnAttack;

        [Button("Attack"), GUIColor(0,0,1)]
        private void OnAttack()
        {
            animator.PlayAttack();
            attackVFX.Play();

            // PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1.0f);
            // for (int i = 0; i < Hit(); ++i)
            // {
            //     _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            // }
        }
    }
}