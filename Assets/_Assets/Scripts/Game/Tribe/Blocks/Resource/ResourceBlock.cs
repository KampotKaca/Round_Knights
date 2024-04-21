using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RoundKnights
{
    public struct ResourceBlockData
    {
        public ulong Amount { get; private set; }
        public ulong Limit { get; private set; }

        public ResourceBlockData(ulong amount, ulong limit)
        {
            Amount = amount;
            Limit = limit;
        }

        public bool IsLimitless => Limit == long.MaxValue;
        public bool IsFull => Space == 0;
        public bool IsEmpty => Amount == 0;
        public ulong Space => Limit - Amount;
        public bool HasEnough(ulong amount) => Amount >= amount;
        public bool HasEnoughSpace(ulong amount) => Space >= amount;
    }
    
    public class ResourceBlock : MonoBehaviour
    {
        [ShowInInspector, ReadOnly, PropertyOrder(100)]
        HashSet<ResourceStorage> m_Storages = new();
        
        #region Events

        [FoldoutGroup("Events"), BoxGroup("Events/Group", ShowLabel = false)] 
        public UnityEvent On_Change;

        #endregion

        #region Storage

        [ShowInInspector, ReadOnly, PropertyOrder(100)]
        readonly List<ResourceBlockData> m_Data = new();

        public ResourceBlockData GetResourceData(ResourceType type) => m_Data[(int)type];

        public void Load()
        {
            int size = Enum.GetValues(typeof(ResourceType)).Length;
            for (int i = 0; i < size; i++) m_Data.Add(new());
        }
        
        public bool ConnectStorage(ResourceStorage storage)
        {
            if (m_Storages.Add(storage))
            {
                onChange();
                storage.e_OnResourceChange.AddListener(onChange);
                return true;
            }

            return false;
        }

        public bool DisconnectStorage(ResourceStorage storage)
        {
            if (m_Storages.Remove(storage))
            {
                onChange();
                storage.e_OnResourceChange.RemoveListener(onChange);
                return true;
            }

            return false;
        }

        void onChange()
        {
            for (int i = 0; i < m_Data.Count; i++)
            {
                ulong amount = 0, limit = 0;

                foreach (var storage in m_Storages)
                {
                    if (!storage.CanStore((ResourceType)i, out var resource)) continue;
                    
                    amount += resource.Amount;
                    if (!resource.IsLimitless && limit != long.MaxValue) limit += resource.Limit;
                    else limit = long.MaxValue;
                }

                m_Data[i] = new(amount, limit);
            }

            On_Change.Invoke();
        }

        void onChange(ResourceType type, ulong change, bool isPositive)
        {
            ulong amount = 0, limit = 0;

            foreach (var storage in m_Storages)
            {
                if (!storage.CanStore(type, out var resource)) continue;
                    
                amount += resource.Amount;
                if (!resource.IsLimitless && limit != long.MaxValue) limit += resource.Limit;
                else limit = long.MaxValue;
            }

            m_Data[(int)type] = new(amount, limit);
            
            On_Change.Invoke();
        }
        
        #endregion
    }
}