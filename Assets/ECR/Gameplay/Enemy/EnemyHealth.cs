using UnityEngine;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UniRx;
using ECR.Gameplay.Logic;

namespace ECR.Gameplay.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] [CanBeNull] private EnemyAnimator animator;
        [SerializeField] [CanBeNull] private HapticSource hitHFX;

        [field: BoxGroup("Health"), SerializeField, ReadOnly] public IntReactiveProperty CurrentHP { get; set; } = new();
        [field: BoxGroup("Health"), SerializeField] public int MaxHP { get; set; }

        [BoxGroup("Health")]
        [Button ("Hit", ButtonStyle.FoldoutButton), GUIColor(1f, 0.6f, 0.4f)]
        public void TakeDamage(int damage)
        {
            if (CurrentHP.Value <= 0)
                return;
            
            CurrentHP.Value -= damage;
            
            animator?.PlayHit();
            hitHFX?.Play();
            
            if (CurrentHP.Value < 0)
                CurrentHP.Value = 0;
        }
    }
}