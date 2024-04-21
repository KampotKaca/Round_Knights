using System;
using UnityEngine;

namespace RoundKnights
{
    public class Building : MonoBehaviour
    {
        public Transform Trs { get; private set; }
        public GameObject GM { get; private set; }
        
        #region Save&Load

        public HealthStat Health { get; private set; }
        
        [Serializable]
        public class SaveFile
        {
            public TransformSaveFile Transform;
        }

        public virtual Type FileType => typeof(SaveFile);
        public Tribe Tribe { get; private set; }
        public bool Loaded { get; private set; }
        
        public virtual void Load(Tribe tribe)
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError($"Trying to load entity {name} twice this is not allowed");
                return;
            }
#endif
            
            Tribe = tribe;
            OnCollect();

            Loaded = true;
        }

        public virtual void Load(SaveFile save, Tribe tribe)
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError($"Trying to load entity {name} twice this is not allowed");
                return;
            }
#endif
            
            Tribe = tribe;
            OnCollect();
            Trs.position = save.Transform.Position;
            Trs.eulerAngles = save.Transform.Euler;

            Loaded = true;
        }

        public virtual SaveFile GetSaveFile()
        {
#if UNITY_EDITOR
            if (!Loaded)
            {
                Debug.LogError($"Trying to load entity {name} which was not loaded");
                return null;
            }
#endif
            
            var save = (SaveFile)Activator.CreateInstance(FileType);
            save.Transform = new();
            save.Transform.SetTransform(Trs);
            return save;
        }

        protected virtual void OnCollect()
        {
            Trs = transform;
            GM = gameObject;
            Health = GetComponent<HealthStat>();
        }

        #endregion
    }
}