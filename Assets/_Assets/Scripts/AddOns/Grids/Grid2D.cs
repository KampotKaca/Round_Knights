using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    [DefaultExecutionOrder(-10)]
    public abstract class Grid2D<T> : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] bool m_AutoLoad = true;
        [SerializeField] float m_CellSize = 10f;
        [SerializeField] Vector2Int m_CellCount = new(100, 100);
        
        #endregion

        #region Accessors
        
        public float CellSize => m_CellSize;
        public Vector2Int CellCount => m_CellCount;
        
        public T this[int x, int y] => m_Cells[x, y];
        public T this[Vector2Int id] => m_Cells[id.x, id.y];
        
        #endregion
        
        #region InnerData
        
        [ShowInInspector, ReadOnly, FoldoutGroup("Info"), BoxGroup("Info/Group", ShowLabel = false)]
        public Vector2 AreaSize { get; private set; }
        [ShowInInspector, ReadOnly, BoxGroup("Info/Group")] 
        public Vector2 Corner { get; private set; }
        
        public bool IsLoaded { get; private set; }
        public Transform Trs { get; private set; }

        float m_DivCellSize;
        
        T[,] m_Cells;
        
        #endregion

        #region Load

        void Awake()
        {
            if(m_AutoLoad) Load();
        }
        
        public void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;
            Trs = transform;

            m_DivCellSize = 1 / m_CellSize;
            AreaSize = (Vector2)m_CellCount * m_CellSize;
            Corner = ToVec2(transform.position) - new Vector2(m_CellCount.x * m_CellSize, m_CellCount.y * m_CellSize) * .5f;

            for (int x = 0; x < m_CellCount.x; x++)
                for (int y = 0; y < m_CellCount.y; y++) m_Cells[x, y] = CreateCell(x, y);
            
            OnLoad();
        }
        
        #endregion

        #region Helpers

        public Vector2 ToLocal(Vector2 position) => position - Corner;

        public Vector2Int GetCellID(Vector3 position) => GetCellID(ToVec2(position));
        public Vector2Int GetCellID(Vector2 position)
        {
            position -= Corner;
            return new()
            {
                x = Mathf.Clamp((int)(position.x * m_DivCellSize), 0, m_CellCount.x - 1),
                y = Mathf.Clamp((int)(position.y * m_DivCellSize), 0, m_CellCount.y - 1),
            };
        }

        public T GetCell(Vector3 position) => this[GetCellID(position)];
        public T GetCell(Vector2 position) => this[GetCellID(position)];

        public bool IsInsideBounds(Vector3 position, out Vector2Int cellId) => IsInsideBounds(ToVec2(position), out cellId);
        public bool IsInsideBounds(Vector2 position, out Vector2Int cellId)
        {
            position -= Corner;
            cellId = new Vector2Int
            {
                x = (int)(position.x * m_DivCellSize),
                y = (int)(position.y * m_DivCellSize),
            };

            return cellId is { x: >= 0, y: >= 0 } && cellId.x < m_CellCount.x && cellId.y < m_CellCount.y;
        }

        public bool IsInsideBounds(Vector3 position, out T cell) => IsInsideBounds(ToVec2(position), out cell);
        public bool IsInsideBounds(Vector2 position, out T cell)
        {
            bool isInside = IsInsideBounds(position, out Vector2Int cellId);
            cell = this[cellId];
            return isInside;
        }
        
        #endregion
        
        #region Abstracts

        protected abstract void OnLoad();
        protected abstract T CreateCell(int x, int y);
        protected abstract Vector3 ToVec3(Vector2 v2);
        protected abstract Vector2 ToVec2(Vector3 v3);
        
        #endregion

        #region Editor

        #if UNITY_EDITOR

        [SerializeField, BoxGroup("Info/Group")] bool m_DrawGizmos;
        [SerializeField, BoxGroup("Info/Group"), ShowIf(nameof(m_DrawGizmos))] Color m_GizmoColor = Color.blue;
        
        protected virtual void OnDrawGizmosSelected()
        {
            if(!m_DrawGizmos) return;
            
            Gizmos.color = m_GizmoColor;
            Vector3 corner = ToVec3(ToVec2(transform.position) - new Vector2(m_CellCount.x * m_CellSize, m_CellCount.y * m_CellSize) * .5f);
            
            //X Axis
            for (int i = 0; i <= m_CellCount.x; i++)
            {
                Gizmos.DrawLine(corner + ToVec3(new Vector2(i * m_CellSize, 0)), 
                                corner + ToVec3(new Vector2(i * m_CellSize, m_CellCount.y * m_CellSize)));
            }
            
            //Z Axis
            for (int i = 0; i <= m_CellCount.y; i++)
            {
                Gizmos.DrawLine(corner + ToVec3(new Vector2(0, i * m_CellSize)), 
                                corner + ToVec3(new Vector2(m_CellCount.y * m_CellSize, i * m_CellSize)));
            }
        }
        
        #endif

        #endregion
    }
}