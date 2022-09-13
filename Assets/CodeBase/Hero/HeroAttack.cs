using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private CharacterController characterController;
        private IInputService _input;

        private readonly Collider[] _hits = new Collider[16];
        private HeroStats _stats;
        private static int _layerMask;
        private float _cleavage;

        private void Awake()
        {
            _input = AllServices.container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !heroAnimator.IsAttacking) 
                heroAnimator.PlayAttack();
        }

        private void OnAttack()
        {
            for (var i = 0; i < Hit(); i++) 
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.damage);
        }

        public void LoadProgress(PlayerProgress progress) =>
            _stats = progress.heroStats;

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.damageRadius, _hits, _layerMask);

        private Vector3 StartPoint() =>
            transform.position.ChangeY(characterController.center.y * 0.5f);
    }
}