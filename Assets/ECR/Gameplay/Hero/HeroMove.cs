using ECR.Services.Input;
using UnityEngine;
using Zenject;

namespace ECR.Gameplay.Hero
{
    public class HeroMove : MonoBehaviour
    {
        private IInputService _inputService;
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed;
        
        private Camera _camera;

        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;

        private void Start() =>
            _camera = Camera.main;

        private void Update()
        {
            var movementVector = Vector3.zero;

            if (_inputService.MoveAxis.sqrMagnitude > 0.001f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.MoveAxis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            characterController.Move(movementVector * (movementSpeed * Time.deltaTime));
        }
    }
}