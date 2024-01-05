using UnityEngine;
using UnityEngine.Events;

using static UnityEngine.InputSystem.InputAction;

namespace ECR.Services.Input
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _controls;
        
        public Vector2 MoveAxis { get; private set; }

        public Vector2 AimAxis { get; private set; }

        public event UnityAction AttackPressed;

        public InputService()
        {
            _controls = new PlayerControls();
            _controls.Enable();
            SubscribeOnControls(true);
        }

        ~InputService()
        {
            SubscribeOnControls(false);
            _controls.Disable();
        }

        private void SubscribeOnControls(bool value)
        {
            if (value)
            {
                _controls.Player.Move.performed += OnMove;
                _controls.Player.Look.performed += OnLook;
                _controls.Player.Fire.performed += OnAttack;
                _controls.Player.Move.canceled  += OnMove;
            }
            else
            {
                _controls.Player.Move.performed -= OnMove;
                _controls.Player.Look.performed -= OnLook;
                _controls.Player.Fire.performed -= OnAttack;
                _controls.Player.Move.canceled  -= OnMove;
            }
        }

        #region Adapter methods

        private void OnMove(CallbackContext ctx) => MoveAxis = ctx.ReadValue<Vector2>();
        private void OnLook(CallbackContext ctx) => AimAxis = ctx.ReadValue<Vector2>();
        private void OnAttack(CallbackContext ctx) => AttackPressed?.Invoke();

        #endregion
    }
}