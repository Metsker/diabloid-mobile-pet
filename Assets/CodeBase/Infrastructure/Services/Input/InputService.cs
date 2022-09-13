using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        private const string AttackButton = "Fire1";
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        
        public abstract Vector2 axis { get; }
        public virtual bool IsAttackButtonUp() =>
            SimpleInput.GetButtonUp(AttackButton);
        protected static Vector2 GetSimpleInputAxis() =>
            new (SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}