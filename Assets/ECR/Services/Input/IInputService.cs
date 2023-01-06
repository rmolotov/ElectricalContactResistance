using UnityEngine;

namespace ECR.Services.Input
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        Vector2 AimAxis { get; }
        bool Fire { get; }
    }
}