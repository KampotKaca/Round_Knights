using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoundKnights
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] Image m_IconImage;
        [SerializeField] TMP_Text m_AmountText;

        ResourceBlock m_Block;
        ResourceType m_Type;
        
        public void Reload(ResourceBlock targetBlock, ResourceType displayType)
        {
            clearEvents();
            m_Block = targetBlock;
            m_Type = displayType;
            setEvents();

            var data = ResourcesConfig.Instance.GetData(m_Type);
            m_IconImage.sprite = data.Icon;
            updateVisual();
        }

        #region Events
        
        void clearEvents()
        {
            if (m_Block)
            {
                m_Block.On_Change.RemoveListener(onChanged);
            }
        }
        
        void setEvents()
        {
            if (m_Block)
            {
                m_Block.On_Change.AddListener(onChanged);
            }
        }
        
        void OnDestroy()
        {
            clearEvents();
        }
        
        #endregion

        void onChanged()
        {
            updateVisual();
        }

        void updateVisual()
        {
            var data = m_Block.GetResourceData(m_Type);
            m_AmountText.text = $"{data.Amount.ToShort()} / {data.Limit.ToShort()}";
        }
    }
}