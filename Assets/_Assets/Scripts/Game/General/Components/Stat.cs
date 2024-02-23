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
        public float Current { get; private set; }
        public bool IsEmpty => Mathf.Approximately(Current, 0);
        public bool IsFull => Mathf.Approximately(Current, 1);
        
        /// <summary>
        /// Amount Is Number
        /// </summary>
        public float CurrentAmount => Current * Config.Limit;

        void Update()
        {
            Current += Mathf.Max(1 - Current, Config.RegenerationSpeed * Time.deltaTime / Config.Limit);
        }

        #region Damage&Mend
        public event Action<float> e_OnStatChange;
        public event Action e_OnEmpty;
        public event Action e_OnFull;
        
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
                e_OnEmpty?.Invoke();
                return true;
            }

            e_OnStatChange?.Invoke(-damagePAmount * Config.Limit);

            return false;
        }

        public bool Mend(float amount)
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
                e_OnFull?.Invoke();
                return true;
            }

            e_OnStatChange?.Invoke(mendPAmount * Config.Limit);

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