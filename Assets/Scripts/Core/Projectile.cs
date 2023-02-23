using Pooling;
using UnityEngine;

namespace Core
{
    [DisallowMultipleComponent]
    public class Projectile : Poolable
    {
        private DamageData _damageData;
        private float _speed;
        private Vector3 _direction;

        public void SetSpeedAndDir(Vector3 dir, float speed)
        {
            _direction = dir;
            _speed = speed;
        }

        public void SetDamageData(DamageData damageData)
        {
            _damageData = damageData;
        }
        
        private void Update()
        {
            transform.position += _direction * (Time.deltaTime * _speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckCollision(collision.collider.gameObject);
        }

        private void CheckCollision(GameObject obj)
        {
            if (obj.TryGetComponent<Entity>(out var e))
            {
                if (e.TakeDamage(_damageData))
                {
                    PoolManager.Despawn(this);   
                }
            }
            else
            {
                PoolManager.Despawn(this);
            }
        }
    }
}
