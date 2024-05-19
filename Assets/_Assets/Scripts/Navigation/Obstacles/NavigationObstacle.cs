using System;
using UnityEngine;

namespace Navigation
{
    public abstract class NavigationObstacle : MonoBehaviour
    {
        public abstract bool Intersects(Bounds bounds, out NavigationHitInfo hitInfo);
        public abstract void EncapsulateIn(ref Bounds bounds, Vector3 surfaceOffset);

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
        }
    }

    public struct NavigationHitInfo
    {
        
    }
}