using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RoundKnights
{
    public class ResourceStorage : MonoBehaviour
    {
        [SerializeField] Resource.InitialCondition[] m_Storages;
        
        [ShowInInspector, ReadOnly, PropertyOrder(100)]
        List<Resource> m_Resources = new();
        
        #region Events

        [FoldoutGroup("Events"), BoxGroup("Events/Group", ShowLabel = false)] 
        public UnityEvent<ResourceType, ulong, bool> e_OnResourceChange;

        #endregion
        
        #region Public Methods

        public bool CanStore(ResourceType type, out Resource resource)
        {
            int id = m_Resources.FindIndex(x => x.Type == type);
            if (id >= 0)
            {
                resource = m_Resources[id];
                return true;
            }

            resource = null;
            return false;
        }

        public Resource GetResource(ResourceType type)
        {
#if UNITY_EDITOR
            var res = m_Resources.Find(x => x.Type == type);
            if(res == null) Debug.LogError($"Cannot UnStored resource ({name}, {type})");
            return res;
#else
            return m_Resources.Find(x => x.Type == type);
#endif
        }

        public ulong Amount(ResourceType type) => GetResource(type).Amount;
        public ulong LimitAmount(ResourceType type) => GetResource(type).Limit;
        
        public bool HasEnough(ResourceType type, ulong amount) => GetResource(type).HasEnough(amount);
        public bool HasEnoughSpace(ResourceType type, ulong amount) => GetResource(type).HasEnoughSpace(amount);

        public ulong Space(ResourceType type) => GetResource(type).Space;
        public bool IsEmpty(ResourceType type) => GetResource(type).IsEmpty;
        public bool HasSpace(ResourceType type) => GetResource(type).HasSpace;

        public bool Add(ResourceType type, int amount) => Add(type, (ulong)Mathf.Max(0, amount));
        public bool Add(ResourceType type, uint amount) => Add(type, (ulong)amount);
        public bool Add(ResourceType type, ulong amount)
        {
            if (!GetResource(type).Add(amount)) return false;
            e_OnResourceChange.Invoke(type, amount, true);
            
            return true;
        }
        
        public bool Remove(ResourceType type, int amount) => Remove(type, (ulong)Mathf.Max(0, amount));
        public bool Remove(ResourceType type, uint amount) => Remove(type, (ulong)amount);
        public bool Remove(ResourceType type, ulong amount)
        {
            if (!GetResource(type).Remove(amount)) return false;
            e_OnResourceChange.Invoke(type, amount, false);
            
            return true;
        }
        
        #endregion
        
        #region Save&Load

        [Serializable]
        public struct SaveFile
        {
            public Resource.SaveFile[] Resources;
        }
        
        public void Load()
        {
            int size = m_Storages.Length;
            for (int i = 0; i < size; i++)
            {
                var type = (ResourceType)i;
                var resId = Array.FindIndex(m_Storages, x => x.Type == type);

                m_Resources.Add(resId < 0 
                    ? new Resource((ResourceType)i) 
                    : new Resource(m_Storages[resId]));
            }

            size = m_Storages.Length;
            for (int i = 0; i < size; i++)
            {
                var res = m_Storages[i];
                m_Resources[(int)res.Type] = new(res);
            }
        }

        public void Load(SaveFile saveFile)
        {
            int size = m_Storages.Length;
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