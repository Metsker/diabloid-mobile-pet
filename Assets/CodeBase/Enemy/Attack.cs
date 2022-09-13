using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class Attack : MonoBehaviour
    {
        private const string LayerName = "Player";
        
        [SerializeField] private EnemyAnimator animator;

        public float damage = 10;
        public float attackCooldown = 2;
        public float cleavage = 0.5f;
        public float effectiveDistance = 0.5f;

        private readonly Collider[] _hits = new Collider[1];
        private Transform _heroTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private bool _isAttackActive;

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void Awake() =>
            _layerMask = 1 << LayerMask.NameToLayer(LayerName);

        private void Update()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
            
            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (!Hit(out Collider hit)) 
                return;
            PhysicsDebug.DrawDebug(StartPoint(), cleavage, 1);
            hit.transform.GetComponent<IHealth>().TakeDamage(damage);
        }

        private void OnAttackEnded()
        {
            _attackCooldown = attackCooldown;
            _isAttacking = false;
        }

        public void EnableAttack() =>
            _isAttackActive = true;

        public void DisableAttack() =>
            _isAttackActive = false;

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();
            
            return hitCount > 0;
        }

        private Vector3 StartPoint() =>
            transform.position.AddY(0.5f) + transform.forward * effectiveDistance;

        private bool CooldownIsUp() =>
            _attackCooldown <= 0;

        private bool CanAttack() =>
            _isAttackActive && !_isAttacking && CooldownIsUp();

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            animator.PlayAttack1();

            _isAttacking = true;
        }
    }
}