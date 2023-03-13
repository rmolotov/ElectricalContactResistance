using System;
using ECR.Gameplay.Logic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using static ECR.Gameplay.Enemy.AttackType;

namespace ECR.Gameplay.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        private readonly BoolReactiveProperty _enabled = new();

        [SerializeField] [CanBeNull] private EnemyAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;

        // TODO: redactor fields and logic
        public AttackType AttackType;
        public int Shield;
        public int AttackDamage;
        public float Cooldown;


        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");

            Observable
                .Interval(TimeSpan.FromSeconds(Cooldown))
                .Where(_ => _enabled.Value)
                .TakeWhile(_ => _enabled.Value != false)
                .TakeUntilDestroy(this)
                .Subscribe(_ => OnAttack());
        }

        public void Activate() => 
            _enabled.Value = true;

        public void Deactivate() => 
            _enabled.Value = false;

        [Button("Attack"), GUIColor(0,0,1)]
        private void OnAttack()
        {
            animator?.PlayAttack();
            attackVFX.Play();
            
            for (var i = 0; i < Hit(from: attackVFX.transform); ++i)
                _hits[i].transform
                    .GetComponentInParent<IHealth>()
                    .TakeDamage(AttackDamage);
        }
        
        private int Hit(Transform from) =>
            AttackType switch
            {
                Direct => Physics.OverlapCapsuleNonAlloc(from.position, from.position + from.forward, DamageRadius, _hits, _layerMask),
                AOE => Physics.OverlapSphereNonAlloc(from.position, DamageRadius, _hits, _layerMask),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}