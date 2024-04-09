using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RoundKnights
{
    public class ResourceBlock : MonoBehaviour
    {
        [Serializable]
        public struct InitialCondition
        {
            public Resource.InitialCondition[] Resources;
        }
        
        [ShowInInspector, ReadOnly, PropertyOrder(100)]
        List<Resource> m_Resources = new();
        
        #region Events

        [FoldoutGroup("Events"), BoxGroup("Events/Group", ShowLabel = false)] 
        public UnityEvent<ResourceType, ulong, bool> e_OnResourceChange;
        [BoxGroup("Events/Group")] 
        public UnityEvent<ResourceType, ulong, bool> e_OnLimitChange;
        [BoxGroup("Events/Group")] 
        public UnityEvent<ResourceType, ulong> e_OnLimitOverflow;

        #endregion
        
        #region Public Methods
        
        public Resource GetResource(ResourceType type) => m_Resources[(int)type];
        public ulong Amount(ResourceType type) => m_Resources[(int)type].Amount;
        public ulong LimitAmount(ResourceType type) => m_Resources[(int)type].Limit;
        
        public bool HasEnough(ResourceType type, ulong amount) => m_Resources[(int)type].HasEnough(amount);
        public bool HasEnoughSpace(ResourceType type, ulong amount) => m_Resources[(int)type].HasEnoughSpace(amount);

        public ulong Space(ResourceType type) => m_Resources[(int)type].Space;
        public bool IsEmpty(ResourceType type) => m_Resources[(int)type].IsEmpty;
        public bool HasSpace(ResourceType type) => m_Resources[(int)type].HasSpace;

        public bool Add(ResourceType type, int amount) => Add(type, (ulong)Mathf.Max(0, amount));
        public bool Add(ResourceType type, uint amount) => Add(type, (ulong)amount);
        public bool Add(ResourceType type, ulong amount)
        {
            if (!m_Resources[(int)type].Add(amount)) return false;

            e_OnResourceChange.Invoke(type, amount, true);
            
            return true;
        }
        
        public bool Remove(ResourceType type, int amount) => Remove(type, (ulong)Mathf.Max(0, amount));
        public bool Remove(ResourceType type, uint amount) => Remove(type, (ulong)amount);
        public bool Remove(ResourceType type, ulong amount)
        {
            if (!m_Resources[(int)type].Remove(amount)) return false;

            e_OnResourceChange.Invoke(type, amount, false);
            
            return true;
        }
        
        public bool AddLimit(ResourceType type, int amount) => AddLimit(type, (ulong)Mathf.Max(0, amount));
        public bool AddLimit(ResourceType type, uint amount) => AddLimit(type, (ulong)amount);
        public bool AddLimit(ResourceType type, ulong amount)
        {
            if (!m_Resources[(int)type].AddLimit(amount)) return false;

            e_OnLimitChange.Invoke(type, amount, true);
            
            return true;
        }
        
        public bool RemoveLimit(ResourceType type, int amount, out ulong overflowAmount) => RemoveLimit(type, (ulong)Mathf.Max(0, amount), out overflowAmount);
        public bool RemoveLimit(ResourceType type, uint amount, out ulong overflowAmount) => RemoveLimit(type, (ulong)amount, out overflowAmount);
        public bool RemoveLimit(ResourceType type, ulong amount, out ulong overflowAmount)
        {
            if (!m_Resources[(int)type].RemoveLimit(amount, out overflowAmount)) return false;

            e_OnLimitChange.Invoke(type, amount, false);
            if (overflowAmount > 0) e_OnLimitOverflow.Invoke(type, overflowAmount);
            
            return true;
        }
        
        #endregion
        
        #region Save&Load

        [Serializable]
        public struct SaveFile
        {
            public Resource.SaveFile[] Resources;
        }
        
        public void Load(InitialCondition condition)
        {
            int size = Enum.GetValues(typeof(ResourceType)).Length;
            for (int i = 0; i < size; i++)
            {
                var type = (ResourceType)i;
                var resId = Array.FindIndex(condition.Resources, x => x.Type == type);

                m_Resources.Add(resId < 0 
                    ? new Resource((ResourceType)i) 
                    : new Resource(condition.Resources[resId]));
            }

            size = condition.Resources.Length;
            for (int i = 0; i < size; i++)
            {
                var res = condition.Resources[i];
                m_Resources[(int)res.Type] = new(res);
            }
        }

        public void Load(SaveFile saveFile)
        {
            int size = Enum.GetValues(typeof(ResourceType)).Length;
            for (int i = 0; i < size; i++)
            {
                var resourceSaveIndex = Array.FindIndex(saveFile.Resources, 
                    x => x.Type == ((ResourceType)i).ToString());
                if (resourceSaveIndex >= 0)
                {
                    m_Resources.Add(new Resource((ResourceType)i, 
                        ref saveFile.Resources[resourceSaveIndex]));
                }
                else m_Resources.Add(new Resource((ResourceType)i));
            }
        }

        public SaveFile GetSaveFile()
        {
            var saveFile = new SaveFile
            {
                Resources = new Resource.SaveFile[m_Resources.Count]
            };

            for (int i = 0; i < m_Resources.Count; i++)
                saveFile.Resources[i] = m_Resources[i].CurrentSaveFile;
            return saveFile;
        }
        
        #endregion
    }
}