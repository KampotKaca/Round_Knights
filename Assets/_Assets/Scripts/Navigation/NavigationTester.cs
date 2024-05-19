using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navigation
{
    public class NavigationTester : MonoBehaviour
    {
        [SerializeField] NavigationSample m_Sample;
        [SerializeField] Transform m_SearchStart, m_SearchFinish;
        
        List<Vector2Int> m_Path;
        SurfaceData m_Surface;
        
        void Awake()
        {
            m_Surface = m_Sample.CreateSurface();
            m_Surface.SetEssentials(transform.position);
        }

        void Update()
        {
            if (m_Path != null)
            {
                for (int i = 1; i < m_Path.Count; i++)
                {
                    Debug.DrawLine(m_Surface.Position(m_Path[i - 1]), m_Surface.Position(m_Path[i]), Color.red);
                }
            }
        }

        [Button]
        void ReDrawPath()
        {
            m_Path = RoundStar.GetPathIndices(m_Surface, 
                m_Surface.GetClampedIndex(m_SearchStart.position), 
               m_Surface.GetClampedIndex(m_SearchFinish.position));
        }

        void OnDrawGizmos()
        {
            if (m_Surface == null) return;
            
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, new Vector3
                    (m_Surface.Nodes.GetLength(0), 0, m_Surface.Nodes.GetLength(1)) * m_Surface.NodeSize);

            Gizmos.color = Color.red;
            for(uint x = 0; x < m_Surface.Nodes.GetLength(0); x++)
            {
                for(uint y = 0; y < m_Surface.Nodes.GetLength(1); y++)
                {
                    if(m_Surface.Nodes[x, y].IsWalkable) continue;
                    Gizmos.DrawWireCube(m_Surface.Position(x, y), Vector3.one * m_Surface.NodeSize);
                }
            }
        }
    }
}