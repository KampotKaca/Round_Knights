using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/Entities/Components/EntityMovement", fileName = "EntityMovement_Config")]
    public class EntityMovementConfig : ScriptableObject
    {
        [field: SerializeField, MinValue(0)] public float Speed { get; private set; } = 5f;
        [field: SerializeField, MinValue(0)] public float AngularSpeed { get; private set; } = 360f;
        [field: SerializeField, MinValue(0)] public float Acceleration { get; private set; } = 12f;
    }
}