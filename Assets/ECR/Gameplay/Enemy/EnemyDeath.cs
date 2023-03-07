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
            animator.PlayDie();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}