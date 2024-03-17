using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class TribesManager : Singleton<TribesManager>, ISavedObject
    {
        [SerializeField, InlineEditor] TribesManagerConfig config;
        
        #region Tribes

        public override void Start()
        {
            base.Start();
            ResourcesDisplayManager.Instance.DisplayBlock(PlayerTribe.ResourceBlock);
        }

        public Tribe PlayerTribe => m_Tribes[0];
        
        readonly List<Tribe> m_Tribes = new();

        void CreateTribe()
        {
            var template = config.TribeTemplate;
            var tribe = Instantiate(template, transform);
            var condition = config.TribeInitialCondition;
            
            condition.IdentifierKey = Guid.NewGuid().ToString();
            
            tribe.Load(condition);
            m_Tribes.Add(tribe);
        }

        void CreateTribe(Tribe.SaveFile saveFile)
        {
            var template = config.TribeTemplate;
            var tribe = Instantiate(template, transform);
            tribe.Load(saveFile);
            m_Tribes.Add(tribe);
        }

        public Tribe FindTribeBy(Predicate<Tribe> condition) => m_Tribes.Find(condition);
        #endregion
        
        #region Save&Load
        public Type FileType => typeof(SaveFile);
        public int Queue => (int)SaveFileQueue.Late;
        public string SaveFileIdentifier => "Tribes_Manager";
        
        [Serializable]
        public struct SaveFile
        {
            public Tribe.SaveFile[] Tribes;
        }
        
        public void LoadDefault()
        {
            CreateTribe();
        }

        public void LoadFromSaveFile(object saveFile)
        {
            var save = (SaveFile)saveFile;
            foreach (var tribe in save.Tribes) CreateTribe(tribe);
        }

        public object GetSaveFile()
        {
            var save = new SaveFile
            {
                Tribes = new Tribe.SaveFile[m_Tribes.Count]
            };
            for (int i = 0; i < m_Tribes.Count; i++) save.Tribes[i] = m_Tribes[i].GetSaveFile();
            return save;
        }
        #endregion
    }
}