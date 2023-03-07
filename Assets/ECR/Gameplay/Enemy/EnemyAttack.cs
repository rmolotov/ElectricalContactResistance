using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator animator;

        // TODO: redactor fields and logic
        public int Shield;
        public IntReactiveProperty AttackDamage = new();
        
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