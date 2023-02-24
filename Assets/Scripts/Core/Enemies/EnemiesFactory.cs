using System;
using System.Collections.Generic;
using Core.Loot;
using Core.Managers;
using Core.Map;
using Pooling;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.Enemies
{
    [DisallowMultipleComponent]
    public class EnemiesFactory : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private GameStats _gameStats;
        [Inject] private LootCoinsFactory _lootCoinsFactory;
        [Inject] private PlayerMovement _player;
        [Inject] private Pathfinding _pathfinding;
        [Inject] private MapGrid _mapGrid;
        
        [Space]
        [SerializeField] private List<BasicEnemy> enemies = new ();
        [SerializeField] private int minEnemies = 2;
        [SerializeField] private int maxEnemies = 6;
        
        private readonly List<BasicEnemy> _spawnedEnemies = new ();

        public event Action OnEnemiesEliminated;

        private void Start()
        {
            SpawnNewEnemies();
            _gameStateManager.OnUpdateRoom += SpawnNewEnemies;
            _gameStateManager.OnDefeat += DespawnAll;
        }

        private void OnDestroy()
        {
            _gameStateManager.OnUpdateRoom -= SpawnNewEnemies;
            _gameStateManager.OnDefeat -= DespawnAll;
        }

        public void SpawnNewEnemies()
        {
#if DEBUG
            if (_spawnedEnemies.Count > 0)
                Debug.LogError("Spawning new enemies when spawned enemies is not empty!");
#endif
            
            var toSpawnCount = Random.Range(minEnemies, maxEnemies + 1);
            var spawnPositions = _mapGrid.GetEnemySpawnPositions(toSpawnCount);

            foreach (var spawnPosition in spawnPositions)
            {
                var randomEnemy = enemies[Random.Range(0, enemies.Count)];
                var enemy = PoolManager.Spawn(randomEnemy, spawnPosition, Quaternion.identity);
                enemy.Setup(_player.transform, _pathfinding, _lootCoinsFactory);

                enemy.OnDeath += HandleEnemyDeath;
                _spawnedEnemies.Add(enemy);
            }
        }

        private void DespawnAll()
        {
            foreach (var i in _spawnedEnemies)
            {
                i.OnDeath -= HandleEnemyDeath;
                PoolManager.Despawn(i);
            }
            _spawnedEnemies.Clear();
        }

        private void HandleEnemyDeath(Entity e)
        {
            e.OnDeath -= HandleEnemyDeath;
            _spawnedEnemies.Remove(e as BasicEnemy);

            if (_gameStateManager.IsGameOver) return; 
            
            _gameStats.AddExp(10);
            if (_spawnedEnemies.Count == 0)
            {
                OnEnemiesEliminated?.Invoke();
            }
        }

        /// <summary>
        /// Find nearest enemy from position throw walls
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rangeDistance">0 - any distance</param>
        /// <returns></returns>
        public Entity GetNearestEnemyFromPosition(Vector3 position, float rangeDistance = 0)
        {
            var nearestSqrDist = float.MaxValue;
            Entity nearest = null;

            if (rangeDistance != 0)
            {
                foreach (var enemy in _spawnedEnemies)
                {
                    var sqrDist = (position - enemy.transform.position).sqrMagnitude;
                    if (sqrDist > rangeDistance * rangeDistance) continue;
                    if (nearestSqrDist < sqrDist) continue;
                    
                    nearestSqrDist = sqrDist;
                    nearest = enemy;
                }

                return nearest;
            }
            
            foreach (var t in _spawnedEnemies)
            {
                var sqrDist = (position - t.transform.position).sqrMagnitude;
                if (nearestSqrDist < sqrDist) continue;
                
                nearestSqrDist = sqrDist;
                nearest = t;
            }
            return nearest;
        }
    }
}
