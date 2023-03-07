using ECR.Gameplay.Logic;
using ECR.Services.Input;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[3];

        private IInputService _inputService;

        [SerializeField] private HeroAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;


        // TODO: redactor fields and logic
        public int Shield;
        [ShowInInspector] public IntReactiveProperty AttackDamage = new();

        [Inject]
        private void Construct(IInputService inputService) 
            => _inputService = inputService;

        private void Start()
        {
            _inputService.AttackPressed += OnAttack;
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");

            AttackDamage.Subscribe(_ =>
            {
                print($"attack set to {AttackDamage.Value}");
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

            for (var i = 0; i < Hit(); ++i)
                _hits[i].transform
                    .GetComponentInParent<IHealth>()
                    .TakeDamage(AttackDamage.Value);
        }

        private int Hit() => 
            Physics.OverlapSphereNonAlloc(attackVFX.transform.position + transform.forward, DamageRadius, _hits, _layerMask);
    }
}