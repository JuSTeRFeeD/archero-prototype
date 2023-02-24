using UnityEngine;

namespace Core.Enemies
{
    public class DashingEnemy : BasicEnemy
    {
        [Header("Dash settings")]
        [SerializeField] private float dashDelay = 1.5f;
        [SerializeField, Range(0, 180)] private float dashAngleDiff = 45f;
        private float _nextDashTime;
        
        protected override void FixedUpdateAttack()
        {
            if (Time.time < _nextDashTime) return;
            _nextDashTime = Time.time + dashDelay;
            
            var diff = PlayerTransform.position - transform.position;
            var angleBetween = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;
            var lowerAngle = angleBetween - dashAngleDiff;
            var upperAngle = angleBetween + dashAngleDiff;
            
            transform.localRotation = Quaternion.Euler(0, Random.Range(lowerAngle, upperAngle), 0);
            Rb.AddForce(transform.forward * moveForceSpeed);
        }
    }
}
