using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace ECR.Services.Input
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _controls;
        
        public Vector2 MoveAxis { get; private set; }

        public Vector2 AimAxis { get; private set; }

        public bool Fire { get; private set; }

        public InputService()
        {
            _controls = new PlayerControls();
            _controls.Enable();
            EnableControls(true);
        }

        ~InputService()
        {
            EnableControls(false);
            _controls.Disable();
            _controls.Dispose();
        }

        private void EnableControls(bool value)
        {
            if (value)
            {
                _controls.Player.Move.performed += Move;
                _controls.Player.Look.performed += Look;
                _controls.Player.Fire.performed += Attack;
                _controls.Player.Move.canceled  += Move;
            }
            else
            {
                _controls.Player.Move.performed -= Move;
                _controls.Player.Look.performed -= Look;
                _controls.Player.Fire.performed -= Attack;
                _controls.Player.Move.canceled  -= Move;
            }
        }

        private void Move(CallbackContext ctx) => MoveAxis = ctx.ReadValue<Vector2>();
        private void Look(CallbackContext ctx) => AimAxis = ctx.ReadValue<Vector2>();
        private void Attack(CallbackContext ctx) => Fire = true;
    }
}