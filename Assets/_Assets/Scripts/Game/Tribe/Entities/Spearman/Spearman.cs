using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoundKnights
{
    public class Spearman : Entity
    {

        #region Save&Load

        protected override Type SaveFileType => typeof(SpearmanSave);

        [Serializable]
        public class SpearmanSave : SaveFile
        {
            
        }

        #endregion
    }
}