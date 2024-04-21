using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class Tribe : MonoBehaviour
    {
        [Serializable]
        public struct InitialCondition
        {
            public string TribeName;
        }
        
        [field: SerializeField, InlineEditor] public TribeConfig Config { get; private set; }
        public ResourceBlock ResourceBlock { get; private set; }
        public BuildingBlock BuildingBlock { get; private set; }
        public EntityBlock EntityBlock { get; private set; }
        
        #region Save&Load
        [Serializable]
        public struct SaveFile
        {
            public string IdentifierKey;
            public string TribeName;
            public BuildingBlock.SaveFile Buildings;
            public EntityBlock.SaveFile Entities;
        }

        public string IdentifierKey { get; private set; }
        public string TribeName => Config.TribeName;

        public void Load()
        {
            IdentifierKey = Guid.NewGuid().ToString();
            
            name = TribeName;
            
            collect();
            
            ResourceBlock.Load();
            BuildingBlock.Load();
            EntityBlock.Load();
        }
        
        public void Load(SaveFile saveFile)
        {
            IdentifierKey = saveFile.IdentifierKey;

            name = TribeName;

            collect();
            
            ResourceBlock.Load();
            BuildingBlock.Load(saveFile.Buildings);
            EntityBlock.Load(saveFile.Entities);
        }

        public SaveFile GetSaveFile()
        {
            SaveFile save = new()
            {
                IdentifierKey = IdentifierKey,
                TribeName = TribeName,
                Buildings = BuildingBlock.GetSaveFile(),
                Entities = EntityBlock.GetSaveFile(),
            };
            return save;
        }
        
        void collect()
        {
            ResourceBlock = GetComponent<ResourceBlock>();
            BuildingBlock = GetComponent<BuildingBlock>();
            EntityBlock = GetComponent<EntityBlock>();
        }
        #endregion
    }
}