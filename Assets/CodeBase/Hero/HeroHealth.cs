using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] 
        private HeroAnimator animator;
        private HeroState _state;
        private HeroStats _stats;
        private bool _isDead;
        public event Action HealthChanged;

        public float Current
        {
            get => _state.currentHP;
            set
            {
                if (!(Math.Abs(_state.currentHP - value) > 0.01f))
                    return;

                _state.currentHP = value;
                HealthChanged?.Invoke();
            }
        }

        public float Max
        {
            get => _stats.maxHP;
            set => _stats.maxHP = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.heroState;
            _stats = progress.heroStats;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.heroState.currentHP = Current;
            progress.heroStats.maxHP = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;
            
            Current -= damage;
            animator.PlayHit();
        }
    }
}