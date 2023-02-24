using System.Collections;
using Core.Loot;
using Core.Map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BasicEnemy : Entity
    {
        private LootCoinsFactory _lootCoinsFactory;
        protected Pathfinding Pathfinding { get; private set; }
        protected Transform PlayerTransform { get; private set; }
        protected Rigidbody Rb;

        public EnemyState EnemyState { get; private set; } = EnemyState.Idle;
        private float _nextUpdateStateTime;
        
        [Header("Basic settings")] 
        [SerializeField] private bool isMovingToPlayer;
        [Space]
        [SerializeField] protected float moveTime;
        [SerializeField, Min(0.1f)] private float idleTime;
        [Space]
        [SerializeField] protected float moveForceSpeed;
        [SerializeField, Min(1)] protected int damage = 20;
        
        [Header("Coins drop")]
        [SerializeField] private int minDropCoins = 0;
        [SerializeField] private int maxDropCoins = 10;

        protected Vector3[] PathToTarget { get; private set; }

        private bool _isSetup;

        private void Start()
        {
            Rb = GetComponent<Rigidbody>();
        }

        public void Setup(Transform playerTransform, Pathfinding pathfinding, LootCoinsFactory lootCoinsFactory)
        {
            _nextUpdateStateTime = Time.time + idleTime;
            if (isMovingToPlayer) StartCoroutine(nameof(UpdatePath));
            if (_isSetup) return;
            _isSetup = true;
            
            PlayerTransform = playerTransform;
            Pathfinding = pathfinding;
            _lootCoinsFactory = lootCoinsFactory;
            
            OnDeath += HandleDeath;
        }
        
        private void HandleDeath(Entity e)
        {
            _lootCoinsFactory.SpawnCoins(Random.Range(minDropCoins, maxDropCoins), transform.position);
            StopCoroutine(nameof(UpdatePath));
        }

        private void FixedUpdate()
        {
            UpdateState();
        }

        protected void LookAtPlayer()
        {
            var diff = PlayerTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(diff, Vector3.up);
        }

        protected virtual void FixedUpdateMoving() { }
        protected virtual void FixedUpdateAttack() { }

        private void UpdateState()
        {
            switch (EnemyState)
            {
                case EnemyState.Idle:
                    FixedUpdateAttack();
                    if (Time.time < _nextUpdateStateTime) return;
                    EnemyState = EnemyState.Moving;
                    _nextUpdateStateTime = Time.time + moveTime;
                    break;
                case EnemyState.Moving:
                    FixedUpdateMoving();
                    if (Time.time < _nextUpdateStateTime) return;
                    EnemyState = EnemyState.Idle;
                    _nextUpdateStateTime = Time.time + idleTime;
                    break;
            }
        }

        private IEnumerator UpdatePath()
        {
            var delay = new WaitForSeconds(1f/10f);
            while (IsAlive)
            {
                if (EnemyState == EnemyState.Idle)
                {
                    yield return null;
                    continue;
                }
                
                PathToTarget = Pathfinding.FindPath(transform.position, PlayerTransform.position);
                yield return delay;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            if (collision.gameObject.TryGetComponent(out Entity player))
            {
                player.TakeDamage(new DamageData
                {
                    Damage = damage
                });
            }
        }
        
#if DEBUG
        private void OnDrawGizmos()
        {
            if (PathToTarget == null || PathToTarget.Length == 0) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, PathToTarget[0]);
            if (PathToTarget.Length <= 1) return;
            for (var i = 0; i < PathToTarget.Length - 1; i++)
            {
                Gizmos.DrawLine(PathToTarget[i], PathToTarget[i + 1]);
            }
        }
#endif
    }
}
