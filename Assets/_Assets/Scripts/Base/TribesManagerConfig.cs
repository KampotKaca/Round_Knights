using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/General/TribesManager", fileName = "TribesManager_Config")]
    public class TribesManagerConfig : ScriptableObject
    {
        [SerializeField, InlineEditor] Tribe[] m_Tribes;

        public int ConditionCount => m_Tribes.Length;

        public Tribe FindTribeBy(Predicate<Tribe> exec) => Array.Find(m_Tribes, exec);
        
        public List<Tribe> SortedConditions(int tribeCount)
        {
            if (ConditionCount < tribeCount)
            {
                Debug.LogError("Not enough conditions were set up");
                return null;
            }
            
            List<Tribe> allConditions = new(m_Tribes);
            List<Tribe> targetConditions = new();

            for (int i = 0; i < tribeCount; i++)
            {
                int id = Random.Range(0, allConditions.Count);
                targetConditions.Add(allConditions[id]);
                allConditions.RemoveAt(id);
            }

            return targetConditions;
        }
    }
}