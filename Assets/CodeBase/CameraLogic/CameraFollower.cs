using System;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private float rotationAngleX;
        [SerializeField] private float distance;
        [SerializeField] private float offsetY;

        private Transform _following;

        private void LateUpdate()
        {
            if (_following == null) return;

            var rotation = Quaternion.Euler(rotationAngleX, 0,0);
            var position = rotation * new Vector3(0, 0, -distance) + GetFollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject followingObject) =>
            _following = followingObject.transform;
        
        private Vector3 GetFollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += offsetY;
            return followingPosition;
        }
    }
}