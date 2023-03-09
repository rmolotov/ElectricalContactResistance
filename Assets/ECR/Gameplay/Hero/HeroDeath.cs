using ECR.Gameplay.Logic;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private GameObject deathVFX;
        
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private HeroHealth health;

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
                .Subscribe(_ => { /* show game over popup */ });
            
            animator.PlayDie();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }
    }
}