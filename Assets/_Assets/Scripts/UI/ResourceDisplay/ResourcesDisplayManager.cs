using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class ResourcesDisplayManager : Singleton<ResourcesDisplayManager>
    {
        [SerializeField] RectTransform m_DisplayParent;
        [SerializeField] ResourceDisplay m_DisplayPrefab;

        readonly List<ResourceDisplay> m_Displays = new();

        [ShowInInspector, ReadOnly] public ResourceBlock DisplayedBlock { get; private set; }

        protected override void OnAwakeEvent()
        {
            var size = Enum.GetValues(typeof(ResourceType)).Length;
            for (int i = 0; i < size; i++)
            {
                var newDisplay = Instantiate(m_DisplayPrefab, m_DisplayParent);
                m_Displays.Add(newDisplay);
            }
        }

        public void DisplayBlock(ResourceBlock block)
        {
            if (DisplayedBlock == block || block == null) return;
            
            DisplayedBlock = block;

            for (int i = 0; i < m_Displays.Count; i++)
                m_Displays[i].Reload(DisplayedBlock, (ResourceType)i);
        }
    }
}