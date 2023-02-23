using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Map
{
    public class Pathfinding
    {
        [Inject] private MapGrid _mapGrid;
        
        public Vector3[] FindPath(Vector3 startWorldPosition, Vector3 targetWorldPosition)
        {
            var startCellPos = _mapGrid.GetCellPositionByWorldPosition(startWorldPosition);
            var targetCellPos = _mapGrid.GetCellPositionByWorldPosition(targetWorldPosition);

            if (!_mapGrid.CheckPositionOnGrid(startCellPos) || !_mapGrid.CheckPositionOnGrid(targetCellPos) ||
                !_mapGrid.Grid[startCellPos.x, startCellPos.y].IsWalkable ||
                !_mapGrid.Grid[targetCellPos.x, targetCellPos.y].IsWalkable)
            {
                return Array.Empty<Vector3>();
            }
            
            var openList = new List<CellNode>();
            var closedList = new List<CellNode>();
            
            openList.Add(_mapGrid.Grid[startCellPos.x, startCellPos.y]);

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
                path = RetracePath(_mapGrid.Grid[startCellPos.x, startCellPos.y], _mapGrid.Grid[targetCellPos.x, targetCellPos.y]);
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
            
            if (nodeGridPos.x - 1 >= 0) neighbors.Add(_mapGrid.Grid[nodeGridPos.x - 1, nodeGridPos.y]);
            if (nodeGridPos.x + 1 < _mapGrid.GridSizeX) neighbors.Add(_mapGrid.Grid[nodeGridPos.x + 1, nodeGridPos.y]);
            if (nodeGridPos.y - 1 >= 0) neighbors.Add(_mapGrid.Grid[nodeGridPos.x, nodeGridPos.y - 1]);
            if (nodeGridPos.y + 1 < _mapGrid.GridSizeY) neighbors.Add(_mapGrid.Grid[nodeGridPos.x, nodeGridPos.y + 1]);

            return neighbors;
        }
    }
}