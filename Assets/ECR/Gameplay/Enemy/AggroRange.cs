using ECR.Gameplay.Logic;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class AggroRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver aggroTrigger;
        [SerializeField] private Follow followComponent;

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
            followComponent.FollowTo();

        private void TriggerExit(Collider other) => 
            followComponent.Stop();
    }
}