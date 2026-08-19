using UnityEngine;

namespace CodeBase.Cameras
{
    public sealed class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Rigidbody target;
        
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 focusOffset;
        
        [SerializeField, Min(1f)] private float distance;
        [SerializeField, Min(0f)] private float smoothTime;
        [SerializeField, Min(0f)] private float lookAheadTime;
        [SerializeField, Min(0f)] private float maxLookAhead;

        private Vector3 velocity;

        private Quaternion Rotation => Quaternion.Euler(rotation);

        private void Start()
        {
            transform.rotation = Rotation;
            transform.position = DesiredPosition();
        }

        private void LateUpdate()
        {
            transform.rotation = Rotation;
            transform.position = Vector3.SmoothDamp(transform.position, DesiredPosition(), ref velocity, smoothTime);
        }

        private Vector3 DesiredPosition()
        {
            var localRotation = Rotation;
            var lookAhead = Mathf.Clamp(-target.linearVelocity.z * lookAheadTime, -maxLookAhead, maxLookAhead);
            var focus = target.position + localRotation * focusOffset + Vector3.back * lookAhead;
            return focus - localRotation * Vector3.forward * distance;
        }
    }
}
