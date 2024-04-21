using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class TribesManager : Singleton<TribesManager>, ISavedObject
    {
        [SerializeField, InlineEditor] TribesManagerConfig m_Config;
        
        #region Tribes

        public override void Start()
        {
            base.Start();
            ResourcesDisplayManager.Instance.DisplayBlock(PlayerTribe.ResourceBlock);
        }

        public Tribe PlayerTribe => m_Tribes[0];
        
        readonly List<Tribe> m_Tribes = new();

        void CreateTribe(Tribe prefab)
        {
            var tribe = Instantiate(prefab, transform);
            
            tribe.Load();
            m_Tribes.Add(tribe);
        }

        void CreateTribe(Tribe prefab, Tribe.SaveFile saveFile)
        {
            var tribe = Instantiate(prefab, transform);
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
            var tribes = m_Config.SortedConditions(1);
            if (tribes == null)
            {
                Application.Quit();
                return;
            }
            
            CreateTribe(tribes[0]);
        }

        public void LoadFromSaveFile(object saveFile)
        {
            var save = (SaveFile)saveFile;
            foreach (var tribeSave in save.Tribes) CreateTribe(m_Config.FindTribeBy(x => x.TribeName == tribeSave.TribeName));
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