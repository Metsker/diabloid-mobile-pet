using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack attack;
        [SerializeField] private TriggerObserver triggerObserver;
    
        private void Start() =>
            attack.DisableAttack();

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

        private void TriggerEnter(Collider obj) =>
            attack.EnableAttack();

        private void TriggerExit(Collider obj) =>
            attack.DisableAttack();
    }
}