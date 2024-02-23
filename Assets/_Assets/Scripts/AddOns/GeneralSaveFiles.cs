using System;
using UnityEngine;

namespace RoundKnights
{
    [Serializable]
    public struct TransformSaveFile
    {
        public float[] Transform;

        public Vector3 Position => new(Transform[0], Transform[1], Transform[2]);
        public Vector3 Euler => new(Transform[3], Transform[4], Transform[5]);

        public void SetTransform(Transform trs)
        {
            Vector3 pos = trs.position, 
                euler = trs.eulerAngles;
            Transform = new[]
            {
                pos.x,   pos.y,   pos.z,
                euler.x, euler.y, euler.z
            };
        }
    }
}