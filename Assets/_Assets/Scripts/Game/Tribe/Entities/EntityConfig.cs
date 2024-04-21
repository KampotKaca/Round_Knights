using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class EntityConfig : ScriptableObject
    {
        protected const string TAB1 = "tab1";
        protected const string GENERAL = "General";
        protected const string STATS = "Stats";
        
        [field: SerializeField, TabGroup(TAB1, GENERAL, SdfIconType.ImageAlt, TextColor = "green")] 
        public string DisplayName { get; private set; } = "Entity";
        [field: SerializeField, TabGroup(TAB1, GENERAL), TextArea] 
        public string Description { get; private set; } = "Entity is Entity";
        [field: SerializeField, TabGroup(TAB1, GENERAL)] 
        public string PoolName { get; private set; } = "Pool";
        [field: SerializeField, TabGroup(TAB1, GENERAL)] 
        public Entity Prefab { get; private set; }
        
        [field: SerializeField, InlineEditor, TabGroup(TAB1, STATS, SdfIconType.BarChartLineFill, TextColor = "blue")] 
        public HealthStatConfig HealthConfig { get; private set; }
        [field: SerializeField, InlineEditor, TabGroup(TAB1, STATS)] 
        public EntityMovementConfig MovementConfig { get; private set; }
    }
}