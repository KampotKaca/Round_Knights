using System;

namespace RoundKnights
{
    public class LordsManor : Building
    {
        public ResourceStorage Storage { get; private set; }
        
        #region Save&Load

        [Serializable]
        public class LordsManorSave : SaveFile
        {
            public ResourceStorage.SaveFile Storage;
        }

        public override Type FileType => typeof(LordsManorSave);
        
        public override void Load(Tribe tribe)
        {
            base.Load(tribe);
            Storage.Load();
            Tribe.ResourceBlock.ConnectStorage(Storage);
        }

        public override void Load(SaveFile save, Tribe tribe)
        {
            base.Load(save, tribe);
            var s = (LordsManorSave)save;
            Storage.Load(s.Storage);
            Tribe.ResourceBlock.ConnectStorage(Storage);
        }

        public override SaveFile GetSaveFile()
        {
            var saveFile = (LordsManorSave)base.GetSaveFile();
            saveFile.Storage = Storage.GetSaveFile();
            
            return saveFile;
        }

        protected override void OnCollect()
        {
            Storage = GetComponent<ResourceStorage>();
            base.OnCollect();
        }

        #endregion
    }
}