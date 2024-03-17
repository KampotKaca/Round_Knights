using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public abstract class Slider : MonoBehaviour
    {
        [SerializeField, OnValueChanged("onFillChanged"), Range(0, 1)] float m_FillAmount = 1;
        
        protected abstract float Color { get; set; }
        protected abstract bool IsSet { get;}
        
        public float FillAmount
        {
            get => m_FillAmount;
            set
            {
                m_FillAmount = value;
                if (IsSet) Color = m_FillAmount;
            }
        }

        #region Editor

        #if UNITY_EDITOR

        protected void onFillChanged()
        {
            FillAmount = m_FillAmount;
            UnityEditor.SceneView.RepaintAll();
        }
        
        #endif

        #endregion
    }
}