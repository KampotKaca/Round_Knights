using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/General/TribesManager", fileName = "TribesManager_Config")]
    public class TribesManagerConfig : ScriptableObject
    {
        [field: SerializeField] public Tribe TribePrefab { get; private set; }
        [SerializeField] Tribe.InitialCondition[] tribeInitialConditions;

        public int ConditionCount => tribeInitialConditions.Length;
        
        public List<Tribe.InitialCondition> SortedConditions(int tribeCount)
        {
            if (ConditionCount < tribeCount)
            {
                Debug.LogError("Not enough conditions were set up");
                return null;
            }
            
            List<Tribe.InitialCondition> allConditions = new(tribeInitialConditions);
            List<Tribe.InitialCondition> targetConditions = new();

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