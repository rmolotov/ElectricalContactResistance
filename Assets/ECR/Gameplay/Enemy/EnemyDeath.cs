using ECR.Gameplay.Logic;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private GameObject deathVFX;
        
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private EnemyHealth health;

        private void Start()
        {
            health.CurrentHP
                .Where(h => h <= 0)
                .Subscribe(_ => Die());
        }

        private void Die()
        {
            Observable
                .FromEvent<AnimatorState>(x => animator.StateExited += x, x => animator.StateExited -= x)
                .Where(state => state == AnimatorState.Death)
                .Subscribe(_ => Destroy(gameObject));
            
            animator.PlayDie();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }
    }
}