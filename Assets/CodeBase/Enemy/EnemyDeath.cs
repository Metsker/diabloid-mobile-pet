using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        private const int DestroyAfterSeconds = 3;

        [SerializeField]
        private EnemyHealth health;
        
        [SerializeField]
        private EnemyHeroChaser enemyHeroChaser;
        
        [SerializeField]
        private EnemyAnimator animator;

        [SerializeField]
        private GameObject deathFx;

        public event Action Happened;

        private void Start() =>
            health.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (health.Current <= 0)
                Die();
        }

        private void Die()
        {
            health.HealthChanged -= HealthChanged;
            
            animator.PlayDeath();

            DisableMovement();

            SpawnDeathFx();
            
            Happened?.Invoke();
            
            StartCoroutine(DestroyTimer());
        }

        private void DisableMovement() =>
            enemyHeroChaser.enabled = false;

        private void SpawnDeathFx() =>
            Instantiate(deathFx, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DestroyAfterSeconds);
            Destroy(gameObject);
        }
    }
}