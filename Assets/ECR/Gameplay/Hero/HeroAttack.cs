﻿using System;
using UnityEngine;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UniRx;
using Zenject;
using ECR.Gameplay.Enemy;
using ECR.Gameplay.Logic;
using ECR.Services.Input;

using static ECR.Gameplay.Enemy.AttackType;

namespace ECR.Gameplay.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private const AttackType AttackType = Direct;
        private readonly Collider[] _hits = new Collider[3];

        private IInputService _inputService;

        [SerializeField] private HeroAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;
        [SerializeField] private HapticSource attackHFX;
        
        [field: SerializeField] public int Shield { get; set; }
        [field: SerializeField] public IntReactiveProperty AttackDamage { get; set; } = new();

        [Inject]
        private void Construct(IInputService inputService) 
            => _inputService = inputService;

        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");

            _inputService.AttackPressed += OnAttack;
            
            AttackDamage.Subscribe(_ =>
            {
                var emission = attackVFX.emission;
                emission.rateOverTimeMultiplier = AttackDamage.Value;
            });
        }

        private void OnDestroy()
        {
            _inputService.AttackPressed -= OnAttack;
        }

        [Button("Attack"), GUIColor(0,0,1)]
        private void OnAttack()
        {
            animator.PlayAttack();
            attackVFX.Play();
            attackHFX.Play();

            for (var i = 0; i < Hit(from: attackVFX.transform); ++i)
                _hits[i].transform
                    .GetComponentInParent<IHealth>()
                    .TakeDamage(AttackDamage.Value);
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