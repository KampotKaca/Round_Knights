using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class EntityMovement : MonoBehaviour
    {
        [field: SerializeField, InlineEditor] public EntityMovementConfig Config { get; private set; }
        
        public float VelocityPercent { get; private set; }
        
        #region Save&Load

        public Transform Trs { get; private set; }
        
        [Serializable]
        public struct SaveFile
        {
            public TransformSaveFile Trs;
        }
        
        public void Load(ref SaveFile saveFile)
        {
            Trs = transform;
            Trs.position = saveFile.Trs.Position;
            Trs.eulerAngles = saveFile.Trs.Euler;
        }

        public SaveFile GetSaveFile()
        {
            SaveFile saveFile = new();
            saveFile.Trs.SetTransform(Trs);
            return saveFile;
        }

        #endregion
    }
}