using UnityEngine;

namespace Navigation
{
    public class NavigationBoxObstacle : NavigationObstacle
    {
        [SerializeField] Bounds m_Bounds = new(Vector3.zero, Vector3.one);
        
        public override bool RayCast(Ray ray, out NavigationHitInfo hitInfo)
        {
            Transform trs = transform;
            var dir = trs.InverseTransformDirection(ray.direction);
            ray = new Ray(ray.origin, dir);
            hitInfo = new();
            return m_Bounds.IntersectRay(ray, out _);
        }
        
        public override void EncapsulateIn(ref Bounds bounds, Vector3 surfaceOffset)
        {
            Transform trs = transform;
            Vector3 pos = trs.position;
            var min = m_Bounds.min;
            var max = m_Bounds.max;

            min = pos + trs.TransformDirection(min) - surfaceOffset;
            max = pos + trs.TransformDirection(max) - surfaceOffset;
            
            bounds.Encapsulate(min);
            bounds.Encapsulate(max);
        }

        #region Editor

        #if UNITY_EDITOR

        readonly Vector3[] m_BoxPoints = new Vector3[8];
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            var trs = transform;
            Vector3 pos = trs.position;
            var min = m_Bounds.min;
            var max = m_Bounds.max;

            m_BoxPoints[0] = pos + trs.TransformDirection(new Vector3(min.x, min.y, min.z));
            m_BoxPoints[1] = pos + trs.TransformDirection(new Vector3(min.x, min.y, max.z));
            m_BoxPoints[2] = pos + trs.TransformDirection(new Vector3(max.x, min.y, min.z));
            m_BoxPoints[3] = pos + trs.TransformDirection(new Vector3(max.x, min.y, max.z));
            
            m_BoxPoints[4] = pos + trs.TransformDirection(new Vector3(min.x, max.y, min.z));
            m_BoxPoints[5] = pos + trs.TransformDirection(new Vector3(min.x, max.y, max.z));
            m_BoxPoints[6] = pos + trs.TransformDirection(new Vector3(max.x, max.y, min.z));
            m_BoxPoints[7] = pos + trs.TransformDirection(new Vector3(max.x, max.y, max.z));

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