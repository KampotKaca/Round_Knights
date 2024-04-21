using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class Stat : MonoBehaviour
    {
        [field: SerializeField, InlineEditor] public StatConfig Config { get; private set; }
        
        /// <summary>
        /// Amount Is Percent!!!
        /// </summary>
        [FoldoutGroup("Info"), BoxGroup("Info/Group", ShowLabel = false), ShowInInspector, ReadOnly, ProgressBar(0, 1)] 
        public float Current { get; private set; }
        
        void Update()
        {
            Current += Mathf.Max(1 - Current, Config.RegenerationSpeed * Time.deltaTime / Config.Limit);
        }
        
        #region Info
        /// <summary>
        /// Amount Is Number
        /// </summary>
        [BoxGroup("Info/Group"), ShowInInspector, ReadOnly] public float CurrentAmount => Config ? Current * Config.Limit : 0;
        [BoxGroup("Info/Group"), ShowInInspector, ReadOnly] public bool IsEmpty => Mathf.Approximately(Current, 0);
        [BoxGroup("Info/Group"), ShowInInspector, ReadOnly] public bool IsFull => Mathf.Approximately(Current, 1);
        
        #endregion

        #region Damage&Heal
        public event Action<float> On_StatChange;
        public event Action On_Empty;
        public event Action On_Full;
        
        [BoxGroup("Info/Group"), Button]
        public bool Damage(float amount)
        {
#if UNITY_EDITOR
            if (amount <= 0)
            {
                Debug.LogWarning($"Trying to damage stat {GetType()}, with amount {amount}");
                return false;
            }
            
            if (IsEmpty)
            {
                Debug.LogWarning($"Trying to damage stat {GetType()}, which is already empty");
                return false;
            }
#endif
            
            float damagePAmount = Mathf.Min(Current, amount / Config.Limit);
            Current -= damagePAmount;
            if (IsEmpty)
            {
                On_Empty?.Invoke();
                return true;
            }

            On_StatChange?.Invoke(-damagePAmount * Config.Limit);

            return false;
        }

        [BoxGroup("Info/Group"), Button]
        public bool Heal(float amount)
        {
#if UNITY_EDITOR
            if (amount <= 0)
            {
                Debug.LogWarning($"Trying to mend stat {GetType()}, with amount {amount}");
                return false;
            }
            
            if (IsFull)
            {
                Debug.LogWarning($"Trying to mend stat {GetType()}, which is already full");
                return false;
            }
#endif

            float mendPAmount = Mathf.Min(1 - Current, amount / Config.Limit);
            Current += mendPAmount;
            if (IsFull)
            {
                On_Full?.Invoke();
                return true;
            }

            On_StatChange?.Invoke(mendPAmount * Config.Limit);

            return false;
        }
        #endregion
        
        #region Save&Load

        [Serializable]
        public class SaveFile
        {
            public float Current;
        }

        protected virtual Type SaveFileType => typeof(SaveFile);
        
        public void Load()
        {
            Current = 1;
        }
        
        public void Load(SaveFile saveFile)
        {
            Current = saveFile.Current;
        }

        public SaveFile GetSaveFile()
        {
            var saveFile = (SaveFile)Activator.CreateInstance(SaveFileType);
            saveFile.Current = Current;
            return saveFile;
        }

        #endregion
    }
}