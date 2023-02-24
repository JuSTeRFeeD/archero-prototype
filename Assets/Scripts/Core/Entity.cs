using System;
using Pooling;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    [DisallowMultipleComponent]
    public class Entity : MonoBehaviour
    {
        [SerializeField] private bool isAlly;
        [SerializeField] private bool despawnOnDeath;
        [SerializeField, Min(1)] private int health;
        private int _currentHealth;
        
        [Space]
        [SerializeField] private VFXPoolItem takeDamageEffect;
        [SerializeField] private VFXPoolItem deathEffect;

        public bool IsAlive => _currentHealth > 0;

        public event Action<Entity> OnDeath;
        public event Action<Entity> OnHealthChange;

        public float HealthPercent => (float)_currentHealth / health;
        public float HealthAmount => _currentHealth;

        private void OnEnable()
        {
            ResetHealth();
        }

        private void ResetHealth()
        {
            _currentHealth = health;
            OnHealthChange?.Invoke(this);
        }

        /// <summary>
        /// Deal damage to current entity
        /// </summary>
        /// <returns>True - if damage taken</returns>
        public bool TakeDamage(DamageData damageData)
        {
            if (isAlly == damageData.IsAlly) return false;

            if (takeDamageEffect != null)
            {
                PoolManager.Spawn(takeDamageEffect, transform.position, Quaternion.identity);
            }
            
            _currentHealth -= damageData.Damage;
            OnHealthChange?.Invoke(this);
            
            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke(this);
                if (deathEffect != null)
                {
                    PoolManager.Spawn(deathEffect, transform.position, Quaternion.identity);
                }
                if (despawnOnDeath) PoolManager.Despawn(this);
            }

            return true;
        }
    }
}
