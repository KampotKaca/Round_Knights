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
            public ResourceBlock.InitialCondition Resources;
            public BuildingBlock.InitialCondition Buildings;
            public EntityBlock.InitialCondition Entities;
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
            public ResourceBlock.SaveFile Resources;
            public BuildingBlock.SaveFile Buildings;
            public EntityBlock.SaveFile Entities;
        }

        public string IdentifierKey { get; private set; }
        public string TribeName { get; private set; }

        public void Load(InitialCondition condition)
        {
            IdentifierKey = Guid.NewGuid().ToString();
            TribeName = condition.TribeName;
            
            name = TribeName;
            
            collect();
            
            ResourceBlock.Load(condition.Resources);
            BuildingBlock.Load(condition.Buildings);
            EntityBlock.Load(condition.Entities);
        }
        
        public void Load(SaveFile saveFile)
        {
            IdentifierKey = saveFile.IdentifierKey;
            TribeName = saveFile.TribeName;

            name = TribeName;

            collect();
            
            ResourceBlock.Load(saveFile.Resources);
            BuildingBlock.Load(saveFile.Buildings);
            EntityBlock.Load(saveFile.Entities);
        }

        public SaveFile GetSaveFile()
        {
            SaveFile save = new()
            {
                IdentifierKey = IdentifierKey,
                TribeName = TribeName,
                Resources = ResourceBlock.GetSaveFile(),
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