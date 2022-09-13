using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService 
    {
        public override Vector2 axis => 
            GetSimpleInputAxis() == Vector2.zero ? GetUnityAxis() : GetSimpleInputAxis();

        public override bool IsAttackButtonUp() =>
            UnityEngine.Input.GetKeyDown(KeyCode.Space) || base.IsAttackButtonUp();

        private static Vector2 GetUnityAxis() =>
            new (UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}