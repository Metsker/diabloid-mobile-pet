using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimator))]
    public class EnemyMovementAnimator : MonoBehaviour
    {
        private const float MinimalVelocity = 0.1f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemyAnimator animator;

        private void Update()
        {
            if (ShouldMove())
                animator.Move(agent.velocity.magnitude);
            else
                animator.StopMoving();
        }

        private bool ShouldMove() =>
            agent.velocity.magnitude > MinimalVelocity && agent.remainingDistance > agent.radius;
    }
}