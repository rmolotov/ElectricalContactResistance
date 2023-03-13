using Sirenix.OdinInspector;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class Follow : MonoBehaviour
    {
        [InfoBox("Select Model transform or aim point for stationary enemies", 
            InfoMessageType.Warning, "@transformForRotate==null")]
        [SerializeField] private Transform transformForRotate;
        private Transform _heroTransform;
        private bool _enabled;

        private void Update()
        {
            if (enabled)
                RotateToHero();
        }

        public void Initialize(Transform hero) =>
            _heroTransform = hero;

        public void FollowTo(Transform hero = null) => 
            (enabled, _heroTransform) = (true, hero ? hero : _heroTransform);

        public void Stop() => 
            (enabled) = (false);
        

        private void RotateToHero()
        {
            var positionDelta = _heroTransform.position - transformForRotate.position;
            positionDelta.y = transformForRotate.position.y;

            transformForRotate.rotation = Quaternion.Lerp(
                transformForRotate.rotation,
                Quaternion.LookRotation(positionDelta),
                2f * Time.deltaTime
            );
        }
    }
}