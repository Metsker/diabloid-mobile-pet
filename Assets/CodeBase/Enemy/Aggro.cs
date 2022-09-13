using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Follow))]
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver triggerObserver;
        public Follow follow;
        public NavMeshAgent navMeshAgent;
        public float cooldown;

        private Vector3 _startPosition;
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            _startPosition = transform.position;
            SwitchFollowOff();
        }

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
        }

        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider obj)
        {
            if (_hasAggroTarget)
                return;
            
            _hasAggroTarget = true;
            
            StopAggroCoroutine();
            
            SwitchFollowOn();
        }

        private void TriggerExit(Collider obj)
        {
            if (!_hasAggroTarget)
                return;
            
            _hasAggroTarget = false;
            
            StartAggroCoroutine();
        }

        private void SwitchFollowOff()
        {
            follow.enabled = false;
            navMeshAgent.destination = _startPosition;
        }

        private void SwitchFollowOn() =>
            follow.enabled = true;

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
                StopCoroutine(_aggroCoroutine);
        }

        private void StartAggroCoroutine() =>
            _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());

        private IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            SwitchFollowOff();
        }
    }
}