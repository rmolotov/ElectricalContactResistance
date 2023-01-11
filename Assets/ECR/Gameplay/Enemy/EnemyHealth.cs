using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        //TODO: refactor props and logic
        public int CurrentHP { get; set; }
        public int MaxHP { get; set; }

        public void TakeDamage(int damage)
        {
            if (CurrentHP <= 0)
                return;
            
            CurrentHP -= damage;
            
            if (CurrentHP < 0)
                CurrentHP = 0;
        }
    }
}