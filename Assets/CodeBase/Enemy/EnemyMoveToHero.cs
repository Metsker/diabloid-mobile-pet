using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class EnemyMoveToHero : Follow
    {
        private const float MinimalDistance = 1;

        [SerializeField]
        private NavMeshAgent agent;

        private void Update() =>
            SetDestinationForAgent();

        private void SetDestinationForAgent()
        {
            if (HeroNotReached())
                agent.destination = HeroTransform.position;
        }

        private bool HeroNotReached() => 
            Vector3.Distance(agent.transform.position, HeroTransform.position) >= MinimalDistance;
    }
}