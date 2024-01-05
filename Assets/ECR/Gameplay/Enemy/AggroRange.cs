using System.Collections.Generic;
using UnityEngine;
using ECR.Gameplay.Logic;

namespace ECR.Gameplay.Enemy
{
    public class AggroRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver aggroTrigger;
        [SerializeField] private List<EnemyFollowBase> enemyFollowComponents;

        private void Start()
        {
            aggroTrigger.TriggerEnter += TriggerEnter;
            aggroTrigger.TriggerExit += TriggerExit;
        }

        private void OnDestroy()
        {
            aggroTrigger.TriggerEnter -= TriggerEnter;
            aggroTrigger.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider other) =>
            enemyFollowComponents
                .ForEach(fc => fc
                    .FollowTo());

        private void TriggerExit(Collider other) => 
            enemyFollowComponents
                .ForEach(fc => fc
                    .Stop());
    }
}