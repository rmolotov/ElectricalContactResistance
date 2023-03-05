using Sirenix.OdinInspector;
using UnityEngine;

namespace ECR.Gameplay.Hero
{
    public class HeroHealth : MonoBehaviour
    {
        [SerializeField] private HeroAnimator heroAnimator;
        
        //TODO: refactor props and logic
        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }

        [Button ("Take damage"), GUIColor(0.5f, 0.5f, 0)]
        public void TakeDamage(int damage)
        {
            if (CurrentHP <= 0)
                return;
            
            CurrentHP -= damage;
            heroAnimator.PlayHit();
            
            if (CurrentHP < 0)
                CurrentHP = 0;
        }
    }
}