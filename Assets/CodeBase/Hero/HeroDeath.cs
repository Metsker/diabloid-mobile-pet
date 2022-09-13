using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroAnimator))]
    [RequireComponent(typeof(HeroAttack))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField]
        private HeroHealth health;
        
        [SerializeField]
        private HeroMovement movement;
        
        [SerializeField]
        private HeroAnimator animator;
        
        [SerializeField]
        private HeroAttack attack;
        
        [SerializeField]
        private GameObject deathFx;
        
        private bool _isDead;

        private void Start() =>
            health.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (AliveOrAlreadyDead())
                return;
            
            Die();
        }

        private bool AliveOrAlreadyDead() =>
            !(health.Current <= 0) || _isDead;

        private void Die()
        {
            _isDead = true;
            
            DisableMovement();
            DisableAttack();
            
            animator.PlayDeath();
            
            SpawnDeathFx();
        }

        private void DisableMovement() =>
            movement.enabled = false;

        private void DisableAttack() =>
            attack.enabled = false;

        private void SpawnDeathFx() =>
            Instantiate(deathFx, transform.position, Quaternion.identity);
    }
}