using System.Collections.Generic;
using Core.Managers;
using Pooling;
using UnityEngine;
using Zenject;

namespace Core.Map
{
    public class MapObstacles : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private MapGrid _mapGrid;

        [SerializeField] private GameObject obstaclePrefab;

        [SerializeField, Min(0.01f)] private float perlinScale = 0.5f; 
        [SerializeField, Range(0f, 1f)] private float spawnFromHeight = 0.5f;

        private List<GameObject> _spawnedObstacles = new ();
        
        private void Start()
        {
            RespawnObstacles();
            _gameStateManager.OnUpdateRoom += RespawnObstacles;
        }

        private void RespawnObstacles()
        {
            DespawnObstacles();
            
            var rndOffsetX = Random.Range(-100f, 100f);
            var rndOffsetY = Random.Range(-100f, 100f);
            var grid = _mapGrid.Grid;
            for (var x = 1; x < _mapGrid.GridSizeX - 1; x++)
            {
                for (var y = 1; y < _mapGrid.GridSizeY - 1; y++)
                {
                    if (!grid[x, y].IsSpawnablePosition) continue;
                    var xPos = rndOffsetX + (float)x / _mapGrid.GridSizeX * perlinScale;
                    var yPos = rndOffsetY + (float)y / _mapGrid.GridSizeY * perlinScale;
                    var noise = Mathf.PerlinNoise(xPos, yPos);
                    if (noise > spawnFromHeight)
                    {
                        grid[x, y].IsWalkable = false;
                        var obstacle = PoolManager.Spawn(obstaclePrefab, grid[x, y].WorldPosition, Quaternion.identity);
                        _spawnedObstacles.Add(obstacle);
                    }
                }
            }
        }

        private void DespawnObstacles()
        {
            
            foreach (var i in _spawnedObstacles)
            {
                var pos = _mapGrid.GetCellPositionByWorldPosition(i.transform.position);
                _mapGrid.Grid[pos.x, pos.y].IsWalkable = true;
                PoolManager.Despawn(i);
            }
            _spawnedObstacles.Clear();
        }
    }
}
