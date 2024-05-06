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

        [SerializeField] Color m_StartColor = Color.gray, m_FinishColor = Color.magenta;
        
        void OnDrawGizmos()
        {
            if (m_Surface == null) return;
            
            drawTransform(m_SearchStart, m_StartColor);
            drawTransform(m_SearchFinish, m_FinishColor);

            void drawTransform(Transform trs, Color col)
            {
                if (trs)
                {
                    Gizmos.color = col;
                    var id = m_Surface.GetClampedIndex(trs.position);
                    var nodePosition = m_Surface.Position(id);

                    var center = nodePosition + new Vector3(m_Surface.NodeSize, 0, m_Surface.NodeSize) * .5f;
                    Gizmos.DrawCube(center, new(m_Surface.NodeSize, 1, m_Surface.NodeSize));
                }
            }
        }
    }
}