using UnityEngine;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using UniRx;
using ECR.Gameplay.Logic;

namespace ECR.Gameplay.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private GameObject deathVFX;
        [SerializeField] [CanBeNull] private HapticSource deathHFX;
        
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private HeroHealth health;

        private void Start()
        {
            health.CurrentHP
                .Where(h => h <= 0)
                .Subscribe(_ => WaitForDie());
        }

        private void WaitForDie()
        {
            Observable
                .FromEvent<AnimatorState>(
                    x => animator.StateExited += x, 
                    x => animator.StateExited -= x)
                .Where(state => state == AnimatorState.Death)
                .Subscribe(_ => Die());

            deathHFX?.Play();
            animator.PlayDie();
        }
        
        private void Die()
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}