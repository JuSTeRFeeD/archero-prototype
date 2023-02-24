using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

namespace Core.Map
{
    public class Pathfinding
    {
        private readonly MapGrid _mapGrid;
        private CellNode[,] _grid;

        [Inject]
        private Pathfinding(MapGrid mapGrid)
        {
            _mapGrid = mapGrid;
        }

        private Vector3 _raycastOffset = new (0, 0.25f, 0);
        private readonly LayerMask _defaultLayerMask = 1 << 0;
        private readonly RaycastHit[] _results = new RaycastHit[3];
        
        public Vector3[] FindPath(Vector3 startWorldPosition, Vector3 targetWorldPosition)
        {
            // Checking the direct path to the player
            var ray = new Ray(
                startWorldPosition + _raycastOffset, 
                startWorldPosition.NormalizedDirectionTo(targetWorldPosition).normalized);
            if (Physics.RaycastNonAlloc(ray, _results, float.MaxValue, _defaultLayerMask) == 1)
            {
                return new []{ targetWorldPosition };
            }
            
            _grid = _mapGrid.Grid;
            var startCellPos = _mapGrid.GetCellPositionByWorldPosition(startWorldPosition);
            var targetCellPos = _mapGrid.GetCellPositionByWorldPosition(targetWorldPosition);

            if (!_mapGrid.CheckPositionOnGrid(startCellPos) || !_mapGrid.CheckPositionOnGrid(targetCellPos) ||
                !_grid[startCellPos.x, startCellPos.y].IsWalkable ||
                !_grid[targetCellPos.x, targetCellPos.y].IsWalkable)
            {
                return Array.Empty<Vector3>();
            }
            
            var openList = new List<CellNode>();
            var closedList = new List<CellNode>();
            
            openList.Add(_grid[startCellPos.x, startCellPos.y]);

            var isPathFounded = false;
            
            while (openList.Count > 0)
            {
                var curNode = openList[0];
                openList.RemoveAt(0);
                
                if (curNode.GridPosition == targetCellPos)
                {
                    isPathFounded = true;
                    break;
                }

                openList.Remove(curNode);
                closedList.Add(curNode);

                foreach (var i in GetNeighborList(curNode))
                {
                    if (!i.IsWalkable ||  closedList.Contains(i)) continue;
                    
                    if (!openList.Contains(i))
                    {
                        i.Parent = curNode;
                        openList.Add(i);
                    }
                }

            }

            var path = Array.Empty<Vector3>();
            if (isPathFounded)
            {
                path = RetracePath(_grid[startCellPos.x, startCellPos.y], _grid[targetCellPos.x, targetCellPos.y]);
            }

            return path;
        }

        private static Vector3[] RetracePath(CellNode startNode, CellNode endNode)
        {
            var path = new List<Vector3>();
            var curNode = endNode;
		
            while (curNode != startNode) {
                path.Add(curNode.WorldPosition);
                curNode = curNode.Parent;
            }

            var result = path.ToArray();
            Array.Reverse(result);
            return result;
        }

        private List<CellNode> GetNeighborList(CellNode node)
        {
            var neighbors = new List<CellNode>();
            var nodeGridPos = node.GridPosition;

            if (nodeGridPos.y - 1 >= 0) neighbors.Add(_grid[nodeGridPos.x, nodeGridPos.y - 1]);
            if (nodeGridPos.y + 1 < _mapGrid.GridSizeY) neighbors.Add(_grid[nodeGridPos.x, nodeGridPos.y + 1]);
            
            if (nodeGridPos.x - 1 >= 0)
            {
                if (nodeGridPos.y - 1 >= 0) neighbors.Add(_grid[nodeGridPos.x - 1, nodeGridPos.y - 1]);
                if (nodeGridPos.y + 1 < _mapGrid.GridSizeY) neighbors.Add(_grid[nodeGridPos.x - 1, nodeGridPos.y + 1]);
                neighbors.Add(_grid[nodeGridPos.x - 1, nodeGridPos.y]);
            }

            if (nodeGridPos.x + 1 < _mapGrid.GridSizeX)
            {
                if (nodeGridPos.y - 1 >= 0) neighbors.Add(_grid[nodeGridPos.x + 1, nodeGridPos.y - 1]);
                if (nodeGridPos.y + 1 < _mapGrid.GridSizeY) neighbors.Add(_grid[nodeGridPos.x + 1, nodeGridPos.y + 1]);
                neighbors.Add(_grid[nodeGridPos.x + 1, nodeGridPos.y]);
            }

            return neighbors;
        }
    }
}