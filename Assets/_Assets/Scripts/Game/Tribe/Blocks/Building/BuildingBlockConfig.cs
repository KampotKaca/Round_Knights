using System;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/Tribe/Blocks/Building", fileName = "BuildingBlock")]
    public class BuildingBlockConfig : ScriptableObject
    {
        [SerializeField] Building[] m_BuildingPrefabs;

        public T GetPrefab<T>() where T : Building => (T)Array.Find(m_BuildingPrefabs, x => x is T);
    }
}