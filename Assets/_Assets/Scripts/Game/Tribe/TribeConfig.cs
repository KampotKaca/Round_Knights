using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/Tribe/Tribe", fileName = "Tribe_Config")]
    public class TribeConfig : ScriptableObject
    {
        protected const string TAB1 = "tab1";
        protected const string GENERAL = "General";
        protected const string STATS = "Stats";
        
        [field: SerializeField, TabGroup(TAB1, GENERAL, SdfIconType.ImageAlt, TextColor = "green")] 
        public Tribe.InitialCondition InitialCondition { get; private set; }

        [field: SerializeField, InlineEditor, TabGroup(TAB1, STATS, SdfIconType.BarChartLineFill, TextColor = "blue")]
        public BuildingBlockConfig BuildingConfig { get; private set; }
        [field: SerializeField, InlineEditor, TabGroup(TAB1, STATS)] 
        public EntityBlockConfig EntityConfig { get; private set; }
        
        public string TribeName => InitialCondition.TribeName;
    }
}