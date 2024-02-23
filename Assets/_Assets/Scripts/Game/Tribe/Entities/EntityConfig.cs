using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class EntityConfig : ScriptableObject
    {
        [field: SerializeField, InlineEditor] public HealthStatConfig HealthConfig { get; private set; }
        [field: SerializeField, InlineEditor] public EntityMovementConfig MovementConfig { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; } = "Entity";
        [field: SerializeField, TextArea] public string Description { get; private set; } = "Entity is Entity";
        [field: SerializeField] public Entity Prefab { get; private set; }
    }
}