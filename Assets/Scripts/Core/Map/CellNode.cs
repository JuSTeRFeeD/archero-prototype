using UnityEngine;

namespace Core.Map
{
    public class CellNode
    {
        public Vector2Int GridPosition  { get; }
        public Vector3 WorldPosition  { get; }
        public bool IsWalkable;
        public bool IsLockedToSpawn;
        public CellNode Parent;

        public bool IsSpawnablePosition => IsWalkable && !IsLockedToSpawn;

        public CellNode(Vector2Int gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
            IsWalkable = true;
            IsLockedToSpawn = false;
        }
    }
}