using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class StatConfig : ScriptableObject
    {
        [field: SerializeField, MinValue(1)] public float Limit { get; private set; } = 100;
        [field: SerializeField, MinValue(0)] public float RegenerationSpeed { get; private set; }
    }
}