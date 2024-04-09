using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class EntityConfig : ScriptableObject
    {
        [field: SerializeField, TabGroup("tab1", "General", SdfIconType.ImageAlt, TextColor = "green")] 
        public string DisplayName { get; private set; } = "Entity";
        [field: SerializeField, TabGroup("tab1", "General"), TextArea] 
        public string Description { get; private set; } = "Entity is Entity";
        [field: SerializeField, TabGroup("tab1", "General")] 
        public string PoolName { get; private set; } = "Pool";
        [field: SerializeField, TabGroup("tab1", "General")] 
        public Entity Prefab { get; private set; }
        
        [field: SerializeField, InlineEditor, TabGroup("tab1", "Stats", SdfIconType.BarChartLineFill, TextColor = "blue")] 
        public HealthStatConfig HealthConfig { get; private set; }
        [field: SerializeField, InlineEditor, TabGroup("tab1", "Stats")] 
        public EntityMovementConfig MovementConfig { get; private set; }
    }
}