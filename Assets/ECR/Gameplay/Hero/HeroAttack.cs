using ECR.Services.Input;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        private IInputService _inputService;

        //[SerializeField] private HeroAnimator _animator;

        // TODO: redactor fields and logic
        public int Shield;
        public int AttackDamage;

        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;
        
        private void Update()
        {
            if (_inputService.Fire)
            {
                //_animator.PlayAttack();
            }
        }

        private void OnAttack()
        {
            // PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1.0f);
            // for (int i = 0; i < Hit(); ++i)
            // {
            //     _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            // }
        }
    }
}