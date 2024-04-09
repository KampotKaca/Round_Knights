using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    public static class ObjectPool
    {
        #region Initialization

        static Transform m_Parent;
        static bool m_Initialized;
        
        public static void Init(Transform parent, PoolSetup.Setting[] settings)
        {
            if(m_Initialized) Dispose();

            m_Parent = parent;
            m_Initialized = true;
            m_Objects = new();

            for (int i = 0; i < settings.Length; i++)
            {
                Queue<PoolObject> queue = new();
                m_Objects.Add(settings[i].Prefab.PoolKey, queue);

                for (int j = 0; j < settings[i].Count; j++)
                {
                    var spawn = Object.Instantiate(settings[i].Prefab, m_Parent);
                    queue.Enqueue(spawn);
                    spawn.Create();
                }
            }
        }
        
        public static void Dispose()
        {
            m_Objects.Clear();
        }
        
        #endregion

        #region Spawn&Return
        
        public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null) where T : PoolObject
        {
            var spawn = Spawn(prefab, parent);
            spawn.Trs.position = pos;
            spawn.Trs.rotation = rot;
            spawn.Trs.localScale = scale;
            return spawn;
        }

        public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot, Transform parent = null) where T : PoolObject
        {
            var spawn = Spawn(prefab, parent);
            spawn.Trs.position = pos;
            spawn.Trs.rotation = rot;
            return spawn;
        }
        
        public static T Spawn<T>(T prefab, Vector3 pos, Transform parent = null) where T : PoolObject
        {
            var spawn = Spawn(prefab, parent);
            spawn.Trs.position = pos;
            return spawn;
        }

        static Dictionary<string, Queue<PoolObject>> m_Objects; 
        
        public static T Spawn<T>(T prefab, Transform parent = null) where T : PoolObject
        {
            if (!m_Objects.TryGetValue(prefab.PoolKey, out var queue))
            {
                queue = new();
                m_Objects.Add(prefab.PoolKey, queue);
            }

            PoolObject spawn;
            if (queue.Count == 0)
            {
                spawn = Object.Instantiate(prefab, parent);
                spawn.Create();
            }
            else spawn = queue.Dequeue();
            
            spawn.Spawn();
            return (T)spawn;
        }

        public static void Return(PoolObject target)
        {
#if UNITY_EDITOR
            if (!target.IsBeingUsed)
            {
                Debug.LogError("Cannot Return Unused Object");
                return;
            }
            
            if (!m_Objects.TryGetValue(target.PoolKey, out var queue))
            {
                Debug.LogError("Cannot Return Object with non existent key");
                return;
            }
#endif
            
            queue.Enqueue(target);
            target.Trs.SetParent(m_Parent);
            target.Return();
        }
        
        #endregion
    }
}