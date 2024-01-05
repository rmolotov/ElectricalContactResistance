using System;
using UnityEngine;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using ECR.Gameplay.Logic;

using static ECR.Gameplay.Enemy.AttackType;

namespace ECR.Gameplay.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        
        private bool _enabled;

        [SerializeField, BoxGroup("Components"), CanBeNull] private EnemyAnimator animator;
        [SerializeField, BoxGroup("Components")] private ParticleSystem attackVFX;
        [SerializeField, BoxGroup("Components")] private HapticSource attackHFX;
        
        [field: SerializeField, BoxGroup("Params")] public AttackType AttackType { get; set; }
        [field: SerializeField, BoxGroup("Params")] public int Shield { get; set; }
        [field: SerializeField, BoxGroup("Params")] public int AttackDamage { get; set; }
        [field: SerializeField, BoxGroup("Params")] public float Cooldown { get; set; }

        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");

            Observable
                .Interval(TimeSpan.FromSeconds(Cooldown))
                .Where(_ => _enabled)
                .TakeWhile(_ => _enabled != false)
                .TakeUntilDestroy(this)
                .Subscribe(_ => OnAttack());
        }

        public void Initialize(GameObject hero) =>
            hero
                .OnDestroyAsObservable()
                .Subscribe(_ => Deactivate());

        public void Activate() =>
            (_enabled) = (true);

        public void Deactivate() =>
            (_enabled) = (false);

        [Button("Attack"), GUIColor(0,0,1)]
        private void OnAttack()
        {
            animator?.PlayAttack();
            attackVFX.Play();
            attackHFX.Play();
            
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