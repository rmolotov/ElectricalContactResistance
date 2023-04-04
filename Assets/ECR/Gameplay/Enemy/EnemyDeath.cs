using System;
using ECR.Gameplay.Logic;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private GameObject deathVFX;
        [SerializeField] [CanBeNull] private HapticSource deathHFX;

        [SerializeField] [CanBeNull] private EnemyAnimator animator;
        [SerializeField] private EnemyHealth health;

        public event Action EnemyDied;

        private void Start() =>
            health.CurrentHP
                .Where(h => h <= 0)
                .Subscribe(_ =>
                {
                    deathHFX?.Play();
                    if (animator) WaitForAnimator();
                    else Die();
                });

        private void WaitForAnimator() =>
            Observable
                .FromEvent<AnimatorState>(x => animator.StateExited += x, x => animator.StateExited -= x)
                .Where(state => state == AnimatorState.Death)
                .DoOnSubscribe(() => animator.PlayDie())
                .Subscribe(_ => Die());

        private void Die()
        {
            deathHFX?.Stop();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            EnemyDied?.Invoke();
            Destroy(gameObject);
        }
    }
}