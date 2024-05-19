using UnityEngine;

namespace Navigation
{
    public class NavigationBoxObstacle : NavigationObstacle
    {
        [SerializeField] Bounds m_Bounds = new(Vector3.zero, Vector3.one);
        
        public override bool Intersects(Bounds bounds, out NavigationHitInfo hitInfo)
        {
            redrawBoxPoints();
            var pos = transform.position;
            foreach(Vector3 point in m_BoxPoints)
            {
                if(bounds.Contains(pos + point)) return true;
            }
            return false;
        }
        
        public override void EncapsulateIn(ref Bounds bounds, Vector3 surfaceOffset)
        {
            var trs = transform;
            redrawBoxPoints();
            foreach(Vector3 point in m_BoxPoints) bounds.Encapsulate(trs.TransformPoint(point));
        }

        private readonly Vector3[] m_BoxPoints = new Vector3[8];
        
        private void redrawBoxPoints()
        {
            var trs = transform;
            Vector3 pos = trs.position;
            var min = m_Bounds.min;
            var max = m_Bounds.max;

            m_BoxPoints[0] = pos + trs.TransformDirection(new(min.x, min.y, min.z));
            m_BoxPoints[1] = pos + trs.TransformDirection(new(min.x, min.y, max.z));
            m_BoxPoints[2] = pos + trs.TransformDirection(new(max.x, min.y, min.z));
            m_BoxPoints[3] = pos + trs.TransformDirection(new(max.x, min.y, max.z));
            
            m_BoxPoints[4] = pos + trs.TransformDirection(new(min.x, max.y, min.z));
            m_BoxPoints[5] = pos + trs.TransformDirection(new(min.x, max.y, max.z));
            m_BoxPoints[6] = pos + trs.TransformDirection(new(max.x, max.y, min.z));
            m_BoxPoints[7] = pos + trs.TransformDirection(new(max.x, max.y, max.z));
        }
        
        #region Editor

        #if UNITY_EDITOR

        
        protected override void OnDrawGizmos()
        {
            if(Application.isPlaying) return;
            
            base.OnDrawGizmos();

            redrawBoxPoints();            

            //Lower Quad
            Gizmos.DrawLine(m_BoxPoints[0], m_BoxPoints[2]);
            Gizmos.DrawLine(m_BoxPoints[2], m_BoxPoints[3]);
            Gizmos.DrawLine(m_BoxPoints[3], m_BoxPoints[1]);
            Gizmos.DrawLine(m_BoxPoints[1], m_BoxPoints[0]);
            
            //Upper Quad
            Gizmos.DrawLine(m_BoxPoints[4], m_BoxPoints[6]);
            Gizmos.DrawLine(m_BoxPoints[6], m_BoxPoints[7]);
            Gizmos.DrawLine(m_BoxPoints[7], m_BoxPoints[5]);
            Gizmos.DrawLine(m_BoxPoints[5], m_BoxPoints[4]);
            
            //Sides
            Gizmos.DrawLine(m_BoxPoints[0], m_BoxPoints[4]);
            Gizmos.DrawLine(m_BoxPoints[1], m_BoxPoints[5]);
            Gizmos.DrawLine(m_BoxPoints[2], m_BoxPoints[6]);
            Gizmos.DrawLine(m_BoxPoints[3], m_BoxPoints[7]);
        }
        
        #endif
        
        #endregion
    }
}