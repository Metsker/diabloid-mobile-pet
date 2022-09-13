using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 axis { get; }
        bool IsAttackButtonUp();
    }
}