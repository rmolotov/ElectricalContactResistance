using Sirenix.OdinInspector;
using UnityEngine;

namespace ECR.Gameplay.Enemy
{
    public class EnemyFollowRotate : EnemyFollowBase
    {
        [InfoBox("Select Model transform or aim point for stationary enemies", 
            InfoMessageType.Warning, "@transformForRotate==null")]
        [SerializeField] private Transform transformForRotate;

        private void Update()
        {
            if (Enabled)
                RotateToHero();
        }

        private void RotateToHero()
        {
            var positionDelta = HeroTransform.position - transformForRotate.position;
            positionDelta.y = transformForRotate.position.y;

            transformForRotate.rotation = Quaternion.Lerp(
                transformForRotate.rotation,
                Quaternion.LookRotation(positionDelta),
                2f * Time.deltaTime
            );
        }
    }
}