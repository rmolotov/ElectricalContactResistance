using UnityEngine;

namespace ECR.Gameplay.Logic
{
    [DisallowMultipleComponent]
    public class AnimationStateReporter : StateMachineBehaviour
    {
        private IAnimationStateReader _reader;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            
            FindReader(animator)
                .OnEnter(stateInfo.shortNameHash);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            
            FindReader(animator)
                .OnExit(stateInfo.shortNameHash);
        }

        private IAnimationStateReader FindReader(Animator animator) =>
            _reader ??= animator.gameObject.GetComponent<IAnimationStateReader>();
    }
}