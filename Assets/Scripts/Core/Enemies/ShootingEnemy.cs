using Pooling;
using UnityEngine;
using Utils;

namespace Core.Enemies
{
    public class ShootingEnemy : BasicEnemy
    {
        [Header("Shooting settings")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField, Min(1)] private float projectileSpeed = 1f;
        [SerializeField, Min(0.1f)] private float shootDelay = 1f;

        private float _nextAttackTime;
        
        protected override void FixedUpdateMoving()
        {
            LookAtPlayer();
            if (PathToTarget == null || PathToTarget.Length == 0)
            {
                Rb.velocity = Vector3.zero;
                return;
            }
            Rb.AddForce((PathToTarget[0] - transform.position).normalized * moveForceSpeed);
        }
        
        protected override void FixedUpdateAttack()
        {
            LookAtPlayer();
            Attack();
        }

        private void Attack()
        {
            if (Time.time < _nextAttackTime) return;
            _nextAttackTime = Time.time + shootDelay;
            
            var projectile = PoolManager.Spawn(projectilePrefab, shootPoint.position, Quaternion.identity);
            var dir = transform.NormalizedDirectionTo(PlayerTransform);
            projectile.SetSpeedAndDir(dir, projectileSpeed);
            projectile.SetDamageData(new DamageData
            {
                Damage = damage
            });
        }
    }
}