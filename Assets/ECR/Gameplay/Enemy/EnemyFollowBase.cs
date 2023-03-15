using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public abstract class EnemyFollowBase : MonoBehaviour
    {
        protected Transform HeroTransform;
        protected bool Enabled;
        
        public virtual void Initialize(Transform hero) =>
            HeroTransform = hero;

        public virtual void FollowTo(Transform hero = null) => 
            (Enabled, HeroTransform) = (true, hero ? hero : HeroTransform);

        public virtual void Stop() => 
            (Enabled) = (false);
    }
}