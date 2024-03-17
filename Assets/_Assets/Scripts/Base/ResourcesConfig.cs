using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public enum ResourceType { Gold, Meat, Wood, Iron }
    [CreateAssetMenu(menuName = "RoundKnights/General/Resources", fileName = "ResourcesConfig")]
    public class ResourcesConfig : SingletonScriptableObject<ResourcesConfig>
    {
        [SerializeField] ResourceData[] m_Data = new ResourceData[4];

        public ResourceData GetData(ResourceType type) => m_Data[(int)type];
        
#if UNITY_EDITOR
        void OnValidate()
        {
            var size = Enum.GetValues(typeof(ResourceType)).Length;
            if (m_Data.Length != size)
            {
                m_Data = new ResourceData[size];
                UnityEditor.EditorUtility.SetDirty(this);
            }
            for (int i = 0; i < size; i++)
            {
                m_Data[i] = new ResourceData
                {
                    Type = (ResourceType)i,
                    Icon = m_Data[i].Icon
                };
            }
        }
#endif
    }

    [Serializable]
    public struct ResourceData
    {
        [BoxGroup("Resource", ShowLabel = false), ReadOnly] public ResourceType Type;
        [BoxGroup("Resource")] public Sprite Icon;
    }
}