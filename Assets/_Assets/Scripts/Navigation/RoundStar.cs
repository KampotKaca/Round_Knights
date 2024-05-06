using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public static class RoundStar
    {
        struct NavigationNode
        {
            //G Cost: cost from starting node
            //H Cost: cost to the target node
            //F Cost: sum of the G and H cost
            
            public Vector2Int ParentNode;
            public uint GCost;
            public uint HCost;

            public bool HasParent => ParentNode != -Vector2Int.one;
            public uint FCost => GCost + HCost;
        }
        
        //General directions
        static readonly Vector2Int 
        s_Left = new(-1, 0), 
        s_LeftBottom = new(-1, -1), 
        s_LeftTop = new(-1, 1),
        s_Top = new(0, 1),
        s_Right = new(1, 0),
        s_RightBottom = new(1, -1),
        s_RightTop = new(1, 1),
        s_Bottom = new(0, -1);
        
        public static List<Vector2Int> GetPathIndices(SurfaceData surface, Vector2Int start, Vector2Int target)
        {
            List<Vector2Int> path = new();
            
            var gridSize = surface.Size;
            var nodes = new NavigationNode[gridSize.x, gridSize.y];
            
            HashSet<Vector2Int> openSet = new();
            HashSet<Vector2Int> closedSet = new();

            nodes[start.x, start.y] = new()
            {
                ParentNode = -Vector2Int.one,
                GCost = 0,
                HCost = DistanceCost(start, target)
            };
            openSet.Add(start);

            Vector2Int currentNode = default;
            while (openSet.Count > 0 && getCurrentNode(out currentNode))
            {
                if (currentNode == target) break;
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                
                createNodesAround(currentNode);
            }

            do
            {
                path.Add(currentNode);
                currentNode = nodes[currentNode.x, currentNode.y].ParentNode;
            } while (currentNode != -Vector2Int.one);
            
            path.Reverse();
            Debug.Log(path.Count);
            
            return path;

            bool getCurrentNode(out Vector2Int targetNode)
            {
                uint hCost = uint.MaxValue;
                uint fCost = uint.MaxValue;
                targetNode = -Vector2Int.one;
                
                foreach (var nodeId in openSet)
                {
                    var node = nodes[nodeId.x, nodeId.y];
                    if (node.FCost < fCost || (node.FCost == fCost && node.HCost < hCost))
                    {
                        targetNode = nodeId;
                        hCost = node.HCost;
                        fCost = node.FCost;
                    }
                }

                return targetNode != -Vector2Int.one;
            }
            void createNodesAround(Vector2Int parentNode)
            {
                bool isLeft = parentNode.x > 0, 
                     isRight = parentNode.x < surface.Size.x - 1, 
                     isBottom = parentNode.y > 0, 
                     isTop = parentNode.y < surface.Size.y - 1;
                
                //Left nodes
                if (isLeft)
                {
                    addNode(parentNode + s_Left);
                    if(isTop) addNode(parentNode + s_LeftTop);
                    if(isBottom) addNode(parentNode + s_LeftBottom);
                }
                
                //Top node
                if(isTop) addNode(parentNode + s_Top);
                
                //Bottom node
                if(isBottom) addNode(parentNode + s_Bottom);
                
                //Right nodes
                if (isRight)
                {
                    addNode(parentNode + s_Right);
                    if(isTop) addNode(parentNode + s_RightTop);
                    if(isBottom) addNode(parentNode + s_RightBottom);
                }

                void addNode(Vector2Int id)
                {
                    if (closedSet.Contains(id) || !surface[id].IsWalkable) return;
                    
                    NavigationNode newNode = new()
                    {
                        ParentNode = parentNode,
                        GCost = nodes[parentNode.x, parentNode.y].GCost + DistanceCost(parentNode, id),
                        HCost = DistanceCost(id, target)
                    };

                    var gCost = nodes[id.x, id.y].GCost;
                    if (gCost == 0 || gCost > newNode.GCost)
                    {
                        nodes[id.x, id.y] = newNode;
                        openSet.Add(id);
                    }
                }
            }
        }
        
        public static uint DistanceCost(Vector2Int from, Vector2Int to)
        {
            const int DIAGONAL_COST = 14;
            const int HORIZONTAL_COST = 10;
                
            int xDist = Mathf.Abs(to.x - from.x);
            int yDist = Mathf.Abs(to.y - from.y);
            int diagonalDist = Mathf.Min(xDist, yDist);
            return (uint)(diagonalDist * DIAGONAL_COST + (xDist + yDist - 2 * diagonalDist) * HORIZONTAL_COST);
        }
    }
}