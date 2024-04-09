using UnityEngine;

namespace RoundKnights
{
    public class BuildingBlock : MonoBehaviour
    {
        [System.Serializable]
        public struct InitialCondition
        {
            
        }
        
        #region Save&Load

        [System.Serializable]
        public struct SaveFile
        {
            
        }

        public void Load(InitialCondition condition)
        {
            
        }
        
        public void Load(SaveFile saveFile)
        {
            
        }

        public SaveFile GetSaveFile()
        {
            SaveFile save = new()
            {

            };
            return save;
        }

        #endregion
    }
}