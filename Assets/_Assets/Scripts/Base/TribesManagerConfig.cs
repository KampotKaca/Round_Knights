using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/General/TribesManager", fileName = "TribesManager_Config")]
    public class TribesManagerConfig : ScriptableObject
    {
        public Tribe TribeTemplate => tribeTemplates[Random.Range(0, tribeTemplates.Length)];
        [SerializeField] Tribe[] tribeTemplates;
        
        public Tribe.SaveFile TribeInitialCondition => tribeInitialConditions[Random.Range(0, tribeInitialConditions.Length)];
        [SerializeField] Tribe.SaveFile[] tribeInitialConditions;
    }
}