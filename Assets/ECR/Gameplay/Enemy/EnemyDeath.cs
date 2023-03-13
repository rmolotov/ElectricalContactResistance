using ECR.Gameplay.Logic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private GameObject deathVFX;
        
        [SerializeField] [CanBeNull] private EnemyAnimator animator;
        [SerializeField] private EnemyHealth health;

        private void Start()
        {
            health.CurrentHP
                .Where(h => h <= 0)
                .Subscribe(_ => WaitForDie());
        }

        private void WaitForDie()
        {
            if (animator)
            {
                Observable
                    .FromEvent<AnimatorState>(x => animator.StateExited += x, x => animator.StateExited -= x)
                    .Where(state => state == AnimatorState.Death)
                    .Subscribe(_ => Die());

                animator.PlayDie();
            }
            else
            {
                Die();
            }
        }

        private void Die()
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}