using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class Entity : MonoBehaviour
    {
        [field: SerializeField, InlineEditor] public EntityConfig Config { get; private set; }
        
        #region Save&Load
        public Transform Trs { get; private set; }
        public EntityMovement Movement { get; private set; }
        public EntityAnimator Animator { get; private set; }
        public HealthStat Health { get; private set; }
        
        [Serializable]
        public class SaveFile
        {
            public EntityMovement.SaveFile Movement;
            public Stat.SaveFile Health;
        }

        public bool Loaded { get; private set; }
        protected virtual Type SaveFileType => typeof(SaveFile);
        
        public virtual void Load(SaveFile saveFile)
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError($"Trying to load entity {name} twice this is not allowed");
                return;
            }
#endif
            
            Trs = transform;
            Movement = GetComponent<EntityMovement>();
            Animator = GetComponentInChildren<EntityAnimator>();
            Health = GetComponent<HealthStat>();
            
            Movement.Load(ref saveFile.Movement);
            Health.Load(saveFile.Health);
            
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

            SaveFile saveFile = (SaveFile)Activator.CreateInstance(SaveFileType);

            saveFile.Movement = Movement.GetSaveFile();
            saveFile.Health = Health.GetSaveFile();
            return saveFile;
        }
        #endregion
    }
}