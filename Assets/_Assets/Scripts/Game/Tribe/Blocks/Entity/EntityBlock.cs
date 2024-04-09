using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class EntityBlock : MonoBehaviour
    {
        [System.Serializable]
        public struct InitialCondition
        {
            
        }

        [SerializeField, InlineEditor] EntityBlockConfig m_Config;
        
        readonly HashSet<Entity> m_Entities = new();
        Transform m_Parent;

        [Button]
        public void SpawnSpearman() => CreateEntity<Spearman>(Vector3.zero, Quaternion.identity);

        public T CreateEntity<T>(Vector3 pos, Quaternion rot) where T : Entity
        {
            var pref = m_Config.Prefab<T>();
            var entity = ObjectPool.Spawn(pref, m_Parent);
            m_Entities.Add(entity);
            entity.Trs.position = pos;
            entity.Trs.rotation = rot;
            entity.Load();

            entity.On_Killed += removeEntity;
            
            return entity;
        }

        void removeEntity(Entity entity) => m_Entities.Remove(entity);
        
        #region Save&Load

        [System.Serializable]
        public struct SaveFile
        {
            public List<Spearman.SpearmanSave> Spearmen;
        }

        public void Load(InitialCondition condition)
        {
            collect();
        }
        
        public void Load(SaveFile saveFile)
        {
            collect();

            var pref = m_Config.Prefab<Spearman>();
            foreach (var spearman in saveFile.Spearmen)
            {
                var spawn = ObjectPool.Spawn(pref, m_Parent);
                m_Entities.Add(spawn);
                spawn.Load(spearman);
                spawn.On_Killed += removeEntity;
            }
        }

        public SaveFile GetSaveFile()
        {
            SaveFile save = new()
            {
                Spearmen = new()
            };

            foreach (var entity in m_Entities)
            {
                switch (entity)
                {
                    case Spearman: save.Spearmen.Add((Spearman.SpearmanSave)entity.GetSaveFile()); break;
                }
            }
            
            return save;
        }

        void collect()
        {
            m_Parent = new GameObject("Entities").transform;
            m_Parent.SetParent(transform);
        }
        
        #endregion
    }
}