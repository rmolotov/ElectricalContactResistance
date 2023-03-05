using UnityEngine;
using UnityEngine.Events;

namespace ECR.Services.Input
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        Vector2 AimAxis { get; }
        UnityAction AttackPressed { get; set; }
    }
}