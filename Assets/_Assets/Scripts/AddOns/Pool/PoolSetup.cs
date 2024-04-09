using System;
using UnityEngine;

namespace RoundKnights
{
    [DefaultExecutionOrder(-1)]
    public class PoolSetup : MonoBehaviour
    {
        [Serializable]
        public struct Setting
        {
            public PoolObject Prefab;
            public uint Count;
        }

        [SerializeField] Setting[] m_Settings;
        
        void Awake()
        {
            ObjectPool.Init(transform, m_Settings);
        }

        void OnDestroy()
        {
            ObjectPool.Dispose();
        }
    }
}