using UnityEngine;
using UnityEngine.AI;

namespace ECR.Gameplay.Enemy
{
    public class EnemyFollowMove : EnemyFollowBase
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemyAnimator animator;

        private void Update()
        {
            animator.PlayMove(agent.velocity.magnitude);
            
            if (Enabled)
                agent.destination = HeroTransform.position;
        }
    }
}