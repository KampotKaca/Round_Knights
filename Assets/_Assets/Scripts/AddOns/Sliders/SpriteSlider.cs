using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class SpriteSlider : Slider
    {
        [SerializeField, OnValueChanged("onFillChanged")] SpriteRenderer m_FillImage;
        
        protected override float Color
        {
            get => m_FillImage.color.a;
            set
            {
                var color = m_FillImage.color;
                color.a = value;
                m_FillImage.color = color;
            }
        }

        protected override bool IsSet => m_FillImage != null;
    }
}