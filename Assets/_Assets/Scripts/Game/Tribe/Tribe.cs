using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class Tribe : MonoBehaviour
    {
        [field: SerializeField, InlineEditor] public TribeConfig Config { get; private set; }
        [field: SerializeField] public ResourceBlock ResourceBlock { get; private set; }
        [field: SerializeField] public BuildingBlock BuildingBlock { get; private set; }
        [field: SerializeField] public EntityBlock EntityBlock { get; private set; }

        #region Save&Load
        [Serializable]
        public struct SaveFile
        {
            [ReadOnly] public string IdentifierKey;
            public string TribeName;
            public ResourceBlock.SaveFile Resources;
            public BuildingBlock.SaveFile Buildings;
            public EntityBlock.SaveFile Entities;
        }

        public string IdentifierKey { get; private set; }
        public string TribeName { get; private set; }

        public void Load(SaveFile saveFile)
        {
            IdentifierKey = saveFile.IdentifierKey;
            TribeName = saveFile.TribeName;

            name = TribeName;
            
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
        #endregion
    }
}