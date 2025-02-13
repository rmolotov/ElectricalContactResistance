﻿using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ECR.Gameplay.Logic;

namespace ECR.Gameplay.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        // animator params
        private static readonly int MoveHash = Animator.StringToHash("ForwardVelocity");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");
        
        //animator states
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _walkingStateHash = Animator.StringToHash("Walk");
        private readonly int _hitStateHash = Animator.StringToHash("Hit");
        private readonly int _deathStateHash = Animator.StringToHash("Die");

        [InfoBox("Select Model transform which is a CHILD of this transform\n This also required for working of AnimationStateReporter", 
            InfoMessageType.Warning, "@animator==null")]
        [SerializeField] private Animator animator;
        
        
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public AnimatorState State { get; private set; }

        #region Play Methods

        [HorizontalGroup("Actions")]
        [Button ("Move", ButtonStyle.FoldoutButton), GUIColor(0.9f, 0.9f, 0.9f)]
        public void PlayMove(float velocity) =>
            animator.SetFloat(MoveHash, velocity);

        [HorizontalGroup("Actions")]
        [Button ("Attack"), GUIColor(0,0,1)]
        public void PlayAttack() =>
            animator.SetTrigger(AttackHash);

        [HorizontalGroup("Actions")]
        [Button ("Hit"), GUIColor(0.5f, 0.5f, 0)]
        public void PlayHit() =>
            animator.SetTrigger(HitHash);

        [HorizontalGroup("Actions")]
        [Button ("Kill"), GUIColor(1f, 0f, 0)]
        public void PlayDie() =>
            animator.SetTrigger(DieHash);

        #endregion

        public void OnEnter(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void OnExit(int stateHash)
        {
            StateExited?.Invoke(State);
        }
        
        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if      (stateHash == _idleStateHash)    state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)  state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash) state = AnimatorState.Walking;
            else if (stateHash == _hitStateHash)     state = AnimatorState.Hit;
            else if (stateHash == _deathStateHash)   state = AnimatorState.Death;
            else                                     state = AnimatorState.Unknown;

            return state;
        }
    }
}