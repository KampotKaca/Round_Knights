using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class Entity : PoolObject
    {
        [field: SerializeField, InlineEditor] public EntityConfig Config { get; private set; }

        public override string PoolKey => Config.PoolName;

        public event Action<Entity> On_Killed;

        protected virtual void OnKilled()
        {
            On_Killed?.Invoke(this);
            On_Killed = null;
        }
        
        #region Save&Load
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
        protected virtual Type FileType => typeof(SaveFile);

        protected override void OnCreate()
        {
            Movement = GetComponent<EntityMovement>();
            Animator = GetComponentInChildren<EntityAnimator>();
            Health = GetComponent<HealthStat>();

            Health.On_Empty += OnKilled;
        }

        public virtual void Load()
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError($"Trying to load entity {name} twice this is not allowed");
                return;
            }
#endif
            Movement.Load();
            Health.Load();

            Loaded = true;
        }
        
        public virtual void Load(SaveFile saveFile)
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError($"Trying to load entity {name} twice this is not allowed");
                return;
            }
#endif
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

            var saveFile = (SaveFile)Activator.CreateInstance(FileType);

            saveFile.Movement = Movement.GetSaveFile();
            saveFile.Health = Health.GetSaveFile();
            return saveFile;
        }
        #endregion
    }
}