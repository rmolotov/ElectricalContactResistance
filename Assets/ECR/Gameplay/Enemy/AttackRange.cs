using ECR.Gameplay.Logic;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class AttackRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver attackTrigger;
        [SerializeField] private EnemyAttack attackComponent;
        
        private void Start()
        {
            attackTrigger.TriggerEnter += TriggerEnter;
            attackTrigger.TriggerExit += TriggerExit;
        }

        private void OnDestroy()
        {
            attackTrigger.TriggerEnter -= TriggerEnter;
            attackTrigger.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider other) => 
            attackComponent.Activate();

        private void TriggerExit(Collider other) => 
            attackComponent.Deactivate();
    }
}