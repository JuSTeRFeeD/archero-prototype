using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Map
{
    [DisallowMultipleComponent]
    public class MapGrid : MonoBehaviour
    {
#if DEBUG
        public bool drawGrid = true;
#endif
        [SerializeField, Min(5)] private int gridSizeX = 11;
        [SerializeField, Min(7)] private int gridSizeY = 15;
        [Space]
        [SerializeField] private Vector2Int playerSpawnPosition = new (5, 3);

        private const float CellSize = 1;
        private static float HalfCell => CellSize / 2;
        public int GridSizeX => gridSizeX;
        public int GridSizeY => gridSizeY;

        public CellNode[,] Grid { get; private set; }

        private void Awake()
        {
#if DEBUG
            if (!CheckPositionOnGrid(playerSpawnPosition))
                Debug.LogError("Incorrect Player Spawn Position (out of grid bounds)!");
#endif
            GenerateGrid();
            AvoidSpawnObstaclesNearPlayerSpawn();
        }

        private void GenerateGrid()
        {
            Grid = new CellNode[gridSizeX, gridSizeY];
            for (var x = 0; x < gridSizeX; x++)
            {
                for (var y = 0; y < gridSizeY; y++)
                {
                    var cellPos = new Vector2Int(x, y);
                    Grid[x, y] = new CellNode(cellPos, GetWorldPositionByCellPosition(cellPos));
                }
            }
        }

        public bool CheckPositionOnGrid(Vector2Int gridPos)
        {
            return gridPos.x >= 0 && gridPos.x < gridSizeX && gridPos.y >= 0 && gridPos.y < gridSizeY;
        }

        private void AvoidSpawnObstaclesNearPlayerSpawn()
        {
            Grid[playerSpawnPosition.x, playerSpawnPosition.y].IsLockedToSpawn = true;
            var directions = new List<Vector2Int>
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(1, 1),
                new(1, -1),
                new(-1, 0),
                new(-1, 1),
                new(-1, -1),
            };
            foreach (var i in directions)
            {
                Grid[playerSpawnPosition.x + i.x, playerSpawnPosition.y + i.y].IsLockedToSpawn = true;
            }
        }

        /// Get possible random spawn position for enemies 
        public IEnumerable<Vector3> GetEnemySpawnPositions(int toSpawnCount)
        {
            var result = new Vector3[toSpawnCount];
            HashSet<Vector2Int> usedPositions = new (); 
            
            for (var i = 0; i < toSpawnCount; i++)
            {
                var rndPos = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(gridSizeY / 3, gridSizeY));
                while (!Grid[rndPos.x, rndPos.y].IsSpawnablePosition || usedPositions.Contains(rndPos))
                {
                    rndPos = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(gridSizeY / 3, gridSizeY));
                }

                usedPositions.Add(rndPos);
                result[i] = GetWorldPositionByCellPosition(rndPos);
            }

            return result;
        }
        
        public Vector3 GetPlayerSpawnPosition()
        {
            return GetWorldPositionByCellPosition(playerSpawnPosition);
        }

        public Vector2Int GetCellPositionByWorldPosition(Vector3 worldPosition)
        {
            worldPosition -= transform.position;
            return new Vector2Int((int)worldPosition.x, (int)worldPosition.z);
        }

        public Vector3 GetWorldPositionByCellPosition(Vector2Int cellPosition)
        {
            return new Vector3(cellPosition.x + HalfCell, 0, cellPosition.y + HalfCell) + transform.position;
        }

#if DEBUG
        private void OnDrawGizmos()
        {
            var size = new Vector3(CellSize, 0, CellSize);

            // if (_grid != null)
            // {
            //     var path = FindPath(pathfindingTestObject.position, _playerMovement.transform.position);
            //     Debug.Log(path.Length);
            //     for (var i = 0; i < path.Length; i++)
            //     {
            //         if (i == 0) Gizmos.color = Color.red;
            //         else if (i == path.Length - 1) Gizmos.color = Color.yellow;
            //         else Gizmos.color = Color.blue;
            //
            //         Gizmos.DrawSphere(path[i], 0.5f);
            //     }
            // }

            if (!drawGrid) return;
            
            for (var x = 0; x < gridSizeX; x++)
            {
                for (var y = 0; y < gridSizeY; y++)
                {
                    if (playerSpawnPosition.x == x && playerSpawnPosition.y == y)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(GetWorldPositionByCellPosition(new Vector2Int(x, y)) + Vector3.up * 0.1f, size);
                    }
                    else
                    {
                        Gizmos.color = y > gridSizeY / 3 ? Color.red : Color.white;
                        Gizmos.DrawWireCube(GetWorldPositionByCellPosition(new Vector2Int(x, y)), size);
                    }

                }
            }
        }
#endif
    }
}
