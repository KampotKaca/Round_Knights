using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navigation
{
    [CreateAssetMenu(menuName = "Navigation/Sample", fileName = "NavigationSample")]
    public partial class NavigationSample : ScriptableObject
    {
        [ReadOnly, SerializeField] SurfaceNode[] m_Nodes = new SurfaceNode[100];
        [ReadOnly, SerializeField, Min(0.01f)] float m_NodeSize = 1f;
        [ReadOnly, SerializeField] Vector2Int m_GridDimensions = new(10, 10);

        public SurfaceData CreateSurface()
        {
            if (m_GridDimensions.x * m_GridDimensions.y != m_Nodes.Length)
            {
                Debug.LogError("Surface is corrupted, please resample it");
                return null;
            }
            
            SurfaceData surface = new()
            {
                Size = m_GridDimensions,
                NodeSize = m_NodeSize,
                Nodes = new SurfaceNode[m_GridDimensions.x, m_GridDimensions.y]
            };

            for (int x = 0; x < m_GridDimensions.x; x++)
            {
                for (int y = 0; y < m_GridDimensions.y; y++)
                {
                    surface.Nodes[x, y] = m_Nodes[x * m_GridDimensions.y + y];
                }
            }

            return surface;
        }
    }

    public class SurfaceData
    {
        public Vector2Int Size;
        public float NodeSize;
        public SurfaceNode[,] Nodes;
        public Vector3 Corner;

        public SurfaceNode this[uint x, uint y] => Nodes[x, y];
        public SurfaceNode this[Vector2Int id] => Nodes[id.x, id.y];

        public Vector3 Position(Vector2Int id) => Position((uint)id.x, (uint)id.y);
        public Vector3 Position(uint x, uint y) => Corner + new Vector3((x + .5f) * NodeSize, 0, (y + .5f) * NodeSize);

        public bool IsInside(Vector3 position, out Vector2Int nodeId)
        {
            position -= Corner;
            nodeId = new()
            {
                x = (int)(position.x * m_DivNodeSize),
                y = (int)(position.z * m_DivNodeSize)
            };

            return !(nodeId.x < 0 || nodeId.x >= Size.x || nodeId.y < 0 || nodeId.y >= Size.y);
        }

        public Vector2Int GetClampedIndex(Vector3 position)
        {
            position -= Corner;
            return new()
            {
                x = Mathf.Clamp((int)(position.x * m_DivNodeSize), 0, Size.x - 1),
                y = Mathf.Clamp((int)(position.z * m_DivNodeSize), 0, Size.y - 1),
            };
        }

        float m_DivNodeSize;
        
        public void SetEssentials(Vector3 center)
        {
            Corner = center - new Vector3(Size.x * .5f * NodeSize, 0, Size.y * .5f * NodeSize);
            m_DivNodeSize = 1 / NodeSize;
        }
    }

    [Serializable]
    public struct SurfaceNode
    {
        public bool IsWalkable;
    }

    public class SurfacePath
    {
        public List<PathPoint> Points;

        public int Length => Points.Count;
        public PathPoint this[int id] => Points[id];
    }

    public struct PathPoint
    {
        public Vector2Int Point;
        public Vector3 Position;
    }

    public struct SampleSettings
    {
        public float NodeSize;
        public LayerMask SampleMask;
        public Transform SurfaceParent;
    }
}