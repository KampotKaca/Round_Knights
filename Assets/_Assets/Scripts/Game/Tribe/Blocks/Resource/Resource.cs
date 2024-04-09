using System;
using Sirenix.OdinInspector;
using UnityEngine;

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
        
        [ShowInInspector, ReadOnly, BoxGroup("Resource"), HideIf(nameof(IsLimitless)),
         Tooltip("condition is for situation when Limit is reduced and resource becomes more than limit")]
        public bool AllowLimitOverflow { get; private set; }

        #endregion

        #region Events

        public event Action<ulong, bool> On_ResourceChanged;
        public event Action<ulong, bool> On_LimitChanged;
        public event Action<ulong> On_LimitOverflow;
        
        #endregion
        
        #region Properties

        public bool IsEmpty => Amount == 0;
        public ulong Space => Limit - Amount;
        public bool HasSpace => Space > 0;
        public bool IsLimitless => Limit == long.MaxValue;

        #endregion

        #region Constructors

        public Resource(ResourceType type, bool allowLimitOverflow = false)
        {
            Type = type;
            Amount = 0;
            Limit = long.MaxValue;
            AllowLimitOverflow = allowLimitOverflow;
        }

        public Resource(ResourceType type, ulong amount, bool allowLimitOverflow = false)
        {
            Type = type;
            Amount = amount;
            Limit = long.MaxValue;
            AllowLimitOverflow = allowLimitOverflow;
        }
        

        public Resource(ResourceType type, ulong amount, ulong limit, bool allowLimitOverflow = false)
        {
            Type = type;
            Amount = amount;
            Limit = limit;
            AllowLimitOverflow = allowLimitOverflow;
        }
        
        public Resource(ResourceType type, ref SaveFile saveFile, bool allowLimitOverflow = false)
        {
            Type = type;
            Amount = saveFile.Amount;
            Limit = saveFile.Limit;
            AllowLimitOverflow = allowLimitOverflow;
        }

        public Resource(InitialCondition condition, bool allowLimitOverflow = false)
        {
            Type = condition.Type;
            Amount = condition.Amount;
            Limit = condition.Limit;
            AllowLimitOverflow = allowLimitOverflow;
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

        public bool AddLimit(ulong amount)
        {
            if (IsLimitless)
            {
#if UNITY_EDITOR
                Debug.LogError("Trying to change limit of limitless resource");
#endif
                return false;
            }

            if (amount == 0) return false;

            Limit += amount;
            On_LimitChanged?.Invoke(amount, true);

            return true;
        }

        public bool RemoveLimit(ulong amount, out ulong overflowAmount)
        {
            overflowAmount = 0;

            if (IsLimitless)
            {
#if UNITY_EDITOR
                Debug.LogError("Trying to change limit of limitless resource");
#endif
                return false;
            }

            if (amount == 0) return false;

            var change = Math.Min(Limit, amount);
            if (change > 0)
            {
                Limit -= change;
                On_LimitChanged?.Invoke(amount, false);

                if (Limit < Amount && !AllowLimitOverflow)
                {
                    overflowAmount = Amount - Limit;
                    Remove(overflowAmount);
                    On_LimitOverflow?.Invoke(overflowAmount);
                }
            }

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