using System;
using Sirenix.OdinInspector;

namespace RoundKnights
{
    public class Resource
    {
        [Serializable]
        public struct InitialCondition
        {
            public ResourceType Type;
            public ulong Amount;
            public ulong Limit;
        }
        
        #region Values

        [ShowInInspector, ReadOnly, BoxGroup("Resource", ShowLabel = false)]
        public ResourceType Type { get; private set; }

        [ShowInInspector, ReadOnly, BoxGroup("Resource")]
        public ulong Amount { get; private set; }

        [ShowInInspector, ReadOnly, BoxGroup("Resource")]
        public ulong Limit { get; private set; }

        #endregion

        #region Events

        public event Action<ulong, bool> On_ResourceChanged;
        
        #endregion
        
        #region Properties

        public bool IsEmpty => Amount == 0;
        public ulong Space => Limit - Amount;
        public bool HasSpace => Space > 0;
        public bool IsLimitless => Limit == long.MaxValue;

        #endregion

        #region Constructors

        public Resource(ResourceType type)
        {
            Type = type;
            Amount = 0;
            Limit = long.MaxValue;
        }

        public Resource(ResourceType type, ulong amount)
        {
            Type = type;
            Amount = amount;
            Limit = long.MaxValue;
        }
        

        public Resource(ResourceType type, ulong amount, ulong limit)
        {
            Type = type;
            Amount = amount;
            Limit = limit;
        }
        
        public Resource(ResourceType type, ref SaveFile saveFile)
        {
            Type = type;
            Amount = saveFile.Amount;
            Limit = saveFile.Limit;
        }

        public Resource(InitialCondition condition)
        {
            Type = condition.Type;
            Amount = condition.Amount;
            Limit = condition.Limit;
        }
        
        #endregion

        #region Public Methods

        public bool HasEnough(ulong amount) => Amount >= amount;
        public bool HasEnoughSpace(ulong amount) => Space >= amount;

        public bool Add(ulong amount)
        {
            if (!HasEnoughSpace(amount) || amount == 0) return false;

            Amount += amount;
            On_ResourceChanged?.Invoke(amount, true);

            return true;
        }

        public bool Remove(ulong amount)
        {
            if (!HasEnough(amount) || amount == 0) return false;

            Amount -= amount;
            On_ResourceChanged?.Invoke(amount, false);

            return true;
        }

        #endregion
        
        #region Saving
        [Serializable]
        public struct SaveFile
        {
            public string Type;
            public ulong Amount;
            public ulong Limit;
        }

        public SaveFile CurrentSaveFile => new()
        {
            Type = Type.ToString(),
            Amount = Amount,
            Limit = Limit
        };

        #endregion
    }
}