using ECR.Gameplay.Logic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] [CanBeNull] private EnemyAnimator animator;

        [FoldoutGroup("Health")]
        [ShowInInspector][InlineProperty][ReadOnly]
        public IntReactiveProperty CurrentHP { get; set; } = new();
        public int MaxHP { get; set; }

        [FoldoutGroup("Health")]
        [Button ("Take damage", ButtonStyle.CompactBox, Expanded = true), GUIColor(1f, 0.6f, 0.4f)]
        public void TakeDamage(int damage)
        {
            if (CurrentHP.Value <= 0)
                return;
            
            CurrentHP.Value -= damage;
            animator?.PlayHit();
            
            if (CurrentHP.Value < 0)
                CurrentHP.Value = 0;
        }
    }
}