using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    public enum ResourceType { Gold, Meat, Wood, Stone }

    public class ResourceBlock : MonoBehaviour, ISavedObject
    {
        [Serializable]
        public struct StartingResource
        {
            public ResourceType Type;
            public ulong StartingAmount;
            public ulong StartingLimit;
        }

        void Awake()
        {
            FillLists();
        }

        [SerializeField] List<StartingResource> resources;

        public class Resource
        {
            public event Action onChange;
            public ulong Amount;
            public ulong Limit;

            public Resource(ulong amount, ulong limit)
            {
                Amount = amount;
                Limit = limit;
            }

            public bool HasLimit => Limit != 0;
            public ulong Space => !HasLimit ? ulong.MaxValue : Limit - Amount;
            public bool HasEnough(ulong price) => Amount >= price;
            public bool HasSpace(ulong amount) => Space >= amount;

            public bool Take(ulong amount)
            {
                if (!HasEnough(amount)) return false;
                Amount -= amount;
                onChange?.Invoke();
                return true;
            }

            public bool Add(ulong amount)
            {
                if (!HasSpace(amount)) return false;
                Amount += amount;
                onChange?.Invoke();
                return true;
            }
        }

        readonly List<Resource> m_Resources = new();

        public Resource GetResource(ResourceType type) => m_Resources[(int)type];
        public ulong Amount(ResourceType type) => m_Resources[(int)type].Amount;
        public ulong Limit(ResourceType type) => m_Resources[(int)type].Limit;

        public bool HasEnough(ResourceType type, ulong price) => m_Resources[(int)type].HasEnough(price);
        public bool HasSpace(ResourceType type, ulong amount) => m_Resources[(int)type].HasSpace(amount);

        public ulong Space(ResourceType type) => m_Resources[(int)type].Space;
        public bool HasLimit(ResourceType type) => m_Resources[(int)type].HasLimit;

        #region Take

        public event Action<ResourceType, ulong> onResourceTaken;

        public bool Take(ResourceType type, int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("Trying to take Invalid Amount!!!");
                return false;
            }

            return Take(type, (ulong)amount);
        }

        public bool Take(ResourceType type, uint amount) => Take(type, (ulong)amount);

        public bool Take(ResourceType type, ulong amount)
        {
            if (!m_Resources[(int)type].Take(amount)) return false;
            onResourceTaken?.Invoke(type, amount);
            return true;
        }

        #endregion

        #region Add

        public event Action<ResourceType, ulong> onResourceAdded;

        public bool Add(ResourceType type, int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("Trying to give Invalid Amount!!!");
                return false;
            }

            return Add(type, (ulong)amount);
        }

        public bool Add(ResourceType type, uint amount) => Add(type, (ulong)amount);

        public bool Add(ResourceType type, ulong amount)
        {
            if (!m_Resources[(int)type].Add(amount)) return false;

            onResourceAdded?.Invoke(type, amount);
            return true;
        }

        #endregion

        #region Save&Load

        bool m_ListsAreFilled;
        
        void FillLists()
        {
            if(m_ListsAreFilled) return;

            m_ListsAreFilled = true;
            int size = Enum.GetValues(typeof(ResourceType)).Length;
            for (int i = 0; i < size; i++) m_Resources.Add(new(0, 0));
        }
        
        [Serializable]
        public class ResourceBlockSave
        {
            public ResourceSave[] SavedResources;
        }
        
        [Serializable]
        public struct ResourceSave
        {
            public string Identifier;
            public ulong Amount;
            public ulong Limit;
        }

        public Type FileType => typeof(ResourceBlockSave);
        public int Queue => (int)SaveFileQueue.Skip;
        public string SaveFileIdentifier => "Resource_Save";
        public void LoadDefault()
        {
            FillLists();

            for (int i = 0; i < resources.Count; i++)
            {
                m_Resources[(int)resources[i].Type].Amount = resources[i].StartingAmount;
                m_Resources[(int)resources[i].Type].Limit = resources[i].StartingLimit;
            }
        }

        public void LoadFromSaveFile(object saveFile)
        {
            FillLists();

            var save = (ResourceBlockSave)saveFile;
            for (int i = 0; i < save.SavedResources.Length; i++)
            {
                var current = save.SavedResources[i];
                if (Enum.TryParse(current.Identifier, out ResourceType type))
                {
                    m_Resources[(int)type].Amount = current.Amount;
                    m_Resources[(int)type].Limit = current.Limit;
                }
                else Debug.LogError($"Cannot parse {current.Identifier}");
            }
        }

        public void PopulateSaveFile(object saveFile)
        {
            var save = (ResourceBlockSave)saveFile;
            int size = Enum.GetValues(typeof(ResourceType)).Length;
            save.SavedResources = new ResourceSave[size];

            for (int i = 0; i < size; i++)
            {
                var type = (ResourceType)i;
                save.SavedResources[i] = new()
                {
                    Identifier = type.ToString(),
                    Amount = Amount(type),
                    Limit = Limit(type)
                };
            }
        }
        
        #endregion
    }
}