using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] float m_CellSize = 10f;
        [SerializeField] Vector2 m_TemplateAreaSize = new(100, 100);

        public Vector2 AreaSize { get; private set; }
        public Vector2Int Size { get; private set; }
        
        GridCell[,] m_Cells;
        
        public void Load()
        {
            var size = m_TemplateAreaSize / m_CellSize;
            Size = new Vector2Int((int)size.x, (int)size.y);
            AreaSize = (Vector2)Size * m_CellSize;
            m_Cells = new GridCell[Size.x, Size.y];

            for (int x = 0; x < Size.x; x++)
            {
                for (int z = 0; z < Size.y; z++)
                {
                    
                }
            }
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector3.zero, m_TemplateAreaSize.ToV3());
        }
    }
}