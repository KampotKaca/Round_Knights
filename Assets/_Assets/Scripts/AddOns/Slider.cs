using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class Slider : MonoBehaviour
    {
        [SerializeField, OnValueChanged("onFillChanged")] SpriteRenderer m_FillImage;
        [SerializeField, OnValueChanged("onFillChanged"), Range(0, 1)] float m_FillAmount = 1;
        
        public float FillAmount
        {
            get => m_FillAmount;
            set
            {
                m_FillAmount = value;
                if (m_FillImage)
                {
                    var color = m_FillImage.color;
                    color.a = m_FillAmount;
                    m_FillImage.color = color;
                }
            }
        }

        #region Editor

        #if UNITY_EDITOR

        void onFillChanged()
        {
            FillAmount = m_FillAmount;
            UnityEditor.SceneView.RepaintAll();
        }
        
        #endif

        #endregion
    }
}