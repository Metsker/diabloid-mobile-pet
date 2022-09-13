using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHeroObserver : Follow
    {
        public float speed;
        
        private Vector3 _positionToLook;

        private void Update() =>
            RotateTowardsHero();

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();
            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
            Quaternion.Lerp(rotation, Quaternion.LookRotation(positionToLook), SpeedFactor());

        private float SpeedFactor() =>
            speed * Time.deltaTime;

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = HeroTransform.position - transform.position;
            _positionToLook = positionDiff.ChangeY(transform.position.y);
        }
    }
}