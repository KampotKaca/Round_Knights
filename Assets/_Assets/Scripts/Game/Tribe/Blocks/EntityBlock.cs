using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    public class EntityBlock : MonoBehaviour
    {
        
        
        #region Save&Load

        [System.Serializable]
        public struct SaveFile
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