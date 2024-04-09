using System;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/Tribe/Blocks/Entity", fileName = "EntityBlock")]
    public class EntityBlockConfig : ScriptableObject
    {
        [SerializeField] Entity[] m_Entities;
        
        public T Prefab<T>() where T : Entity => (T)Array.Find(m_Entities, x => x is T);
    }
}