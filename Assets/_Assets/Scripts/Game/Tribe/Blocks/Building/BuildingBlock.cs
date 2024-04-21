using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class BuildingBlock : MonoBehaviour
    {
        [SerializeField, InlineEditor] BuildingBlockConfig m_Config;

        readonly HashSet<Building> m_Buildings = new();
        Transform m_Parent;

        public Tribe Tribe { get; private set; }
        
        public T Build<T>() where T : Building
        {
            var building = build<T>();
            building.Load(Tribe);
            
            return building;
        }
        
        public T Build<T>(Building.SaveFile saveFile) where T : Building
        {
            var building = build<T>();
            building.Load(saveFile, Tribe);
            
            return building;
        }
        
        T build<T>() where T : Building
        {
            var prefab = m_Config.GetPrefab<T>();
            var building = Instantiate(prefab, m_Parent);
            m_Buildings.Add(building);
            
            return building;
        }
        
        #region Save&Load

        [System.Serializable]
        public struct SaveFile
        {
            public LordsManor.LordsManorSave LordsManor;
        }

        public void Load()
        {
            collect();
            var manor = Build<LordsManor>();
            manor.transform.position = Vector3.zero;
        }
        
        public void Load(SaveFile saveFile)
        {
            collect();
            Build<LordsManor>(saveFile.LordsManor);
        }

        public SaveFile GetSaveFile()
        {
            SaveFile save = new();

            LordsManor manor = null;

            foreach (var building in m_Buildings)
            {
                switch (building)
                {
                    case LordsManor m:
                        if (manor) Debug.LogError("Second Lords manor found this is not allowed");
                        else manor = m;
                        break;
                }
            }

            if (!manor) Debug.LogError("No Lords manor found this is not allowed");
            else save.LordsManor = (LordsManor.LordsManorSave)manor.GetSaveFile();
            
            return save;
        }
        
        void collect()
        {
            Tribe = GetComponent<Tribe>();
            m_Parent = new GameObject("Buildings").transform;
            m_Parent.SetParent(transform);
        }

        #endregion
    }
}