using System;
using System.ComponentModel;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField]
        private EnemyAnimator animator;
        
        [field: SerializeField] 
        public float Current { get; set; }
        
        [field: SerializeField] 
        public float Max { get; set; }

        public event Action HealthChanged;

        public void TakeDamage(float damage)
        {
            Current -= damage;
            
            animator.PlayHit();
            
            HealthChanged?.Invoke();
        }
    }
}