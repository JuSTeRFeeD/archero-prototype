using Core.Enemies;
using Pooling;
using UnityEngine;
using Utils;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(PlayerMovement)), DisallowMultipleComponent]
    public class PlayerAttack : MonoBehaviour
    {
        [Inject] private EnemiesFactory _enemiesFactory;
        private PlayerMovement _playerMovement;
        private Transform _target;
        
        private Animator _animator;
        private static readonly int AttackAnim = Animator.StringToHash("attack");
        private static readonly int AttackSpeedAmin = Animator.StringToHash("attackSpeed");

        [Space]
        [SerializeField] private Transform shootPosition;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float attackCooldown = 3;
        private const float BasicAttackSpeed = 0.5f;
        private float _nextAttackTime;

        [SerializeField] private int damage = 10;
        [SerializeField] private int projectileSpeed = 10;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (_playerMovement.IsMoving()) return;
            
            FindNearestEnemy();
            CheckCanAttack();
        }

        private void Update()
        {
            if (_playerMovement.IsMoving()) return;
            
            RotateToEnemy();
        }

        private void RotateToEnemy()
        {
            if (_target == null) return;

            var diff = _target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(diff, Vector3.up);
        }
        
        private void FindNearestEnemy()
        {
            var nearestEnemy = _enemiesFactory.GetNearestEnemyFromPosition(transform.position);
            if (nearestEnemy == null)
            {
                _target = null;
                return;
            }

            _target = nearestEnemy.transform;
        }

        private void CheckCanAttack()
        {
            if (_target == null || Time.time < _nextAttackTime) return;

            _nextAttackTime = Time.time + attackCooldown;
            _animator.SetFloat(AttackSpeedAmin, BasicAttackSpeed / attackCooldown);
            _animator.SetTrigger(AttackAnim);
        }
        
        // Invokes from attack animation
        private void Attack()
        {
            var direction = _target == null 
                ? transform.forward 
                : transform.NormalizedDirectionTo(_target);
            
            var projectile = PoolManager.Spawn(projectilePrefab, shootPosition.position, Quaternion.identity);
            
            projectile.SetSpeedAndDir(direction, projectileSpeed);
            projectile.SetDamageData(new DamageData
            {
                Damage = damage,
                IsAlly = true
            });
        }

        private void OnDrawGizmos()
        {
            if (_target == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}
