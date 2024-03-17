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
        Resource m_Resource;
        
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
            if (m_Resource != null)
            {
                m_Resource.e_OnResourceChanged -= onResourceChanged;
                m_Resource.e_OnLimitChanged -= onLimitChanged;
            }
        }
        
        void setEvents()
        {
            m_Resource = m_Block.GetResource(m_Type);
            m_Resource.e_OnResourceChanged += onResourceChanged;
            m_Resource.e_OnLimitChanged += onLimitChanged;
        }
        
        void OnDestroy()
        {
            clearEvents();
        }
        
        #endregion

        void onResourceChanged(ulong change, bool isPositive)
        {
            updateVisual();
        }
        
        void onLimitChanged(ulong change, bool isPositive)
        {
            updateVisual();
        }

        void updateVisual()
        {
            m_AmountText.text = m_Resource.Amount.ToString();
        }
    }
}