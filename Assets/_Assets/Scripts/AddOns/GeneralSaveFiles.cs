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
    
    [Serializable]
    public struct Vec3SaveFile
    {
        float[] val;

        public Vector3 Value
        {
            get => new(val[0], val[1], val[2]);
            set => val = new[] { value.x, value.y, value.z };
        }
    }
}