using UnityEngine;

namespace CodeBase.Enemy
{
    public static class PhysicsDebug
    {
        public static void DrawDebug(Vector3 start, float radius, float seconds)
        {
            Debug.DrawRay(start, radius * Vector3.up, Color.yellow, seconds);
            Debug.DrawRay(start, radius * Vector3.down, Color.yellow, seconds);
            Debug.DrawRay(start, radius * Vector3.forward, Color.yellow, seconds);
            Debug.DrawRay(start, radius * Vector3.back, Color.yellow, seconds);
            Debug.DrawRay(start, radius * Vector3.left, Color.yellow, seconds);
            Debug.DrawRay(start, radius * Vector3.right, Color.yellow, seconds);
        }
    }
}