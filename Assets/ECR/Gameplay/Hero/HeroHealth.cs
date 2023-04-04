using ECR.Gameplay.Logic;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace ECR.Gameplay.Hero
{
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private HeroAnimator animator;
        [SerializeField] [CanBeNull] private HapticSource hitHFX;

        [BoxGroup("Health")]
        [ShowInInspector][InlineProperty][ReadOnly]
        public IntReactiveProperty CurrentHP { get; set; } = new();
        public int MaxHP { get; set; }

        [BoxGroup("Health")]
        [Button ("Take damage", ButtonStyle.CompactBox, Expanded = true), GUIColor(1f, 0.6f, 0.4f)]
        public void TakeDamage(int damage)
        {
            if (CurrentHP.Value <= 0)
                return;
            
            CurrentHP.Value -= damage;
            
            animator.PlayHit();
            hitHFX?.Play();
            
            if (CurrentHP.Value < 0)
                CurrentHP.Value = 0;
        }
    }
}