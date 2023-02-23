using UnityEngine;

namespace Core
{
    [DisallowMultipleComponent]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [Space] 
        [SerializeField] private float smoothTime;
        [SerializeField] private float maxSpeed;
        private Vector3 _velocity;
        
        private void Update()
        {
            var pos = target.position;
            pos.x = 0;
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref _velocity, smoothTime, maxSpeed);
        }
    }
}
