using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] Image m_imageBackground;
    [SerializeField] Image m_imageItem;
    [SerializeField] TMP_Text m_nameTxt;
    [SerializeField] TMP_Text m_valueTxt;
    [SerializeField] TMP_Text m_infoTxt;

    public void Set_Info(ItemData m_item)
    {
        m_imageItem.sprite = m_item.itemSprite;
        m_imageBackground.sprite = m_item.shadowSprite;

        m_nameTxt.text = m_item.itemName;

        m_valueTxt.text = "¼Ó¼º : ";
        switch (m_item.propertyType)
        {
            case PROPERTYTYPE.PT_RUIN:
                m_valueTxt.text = "ÆÄ±«";
                break;
            case PROPERTYTYPE.PT_ZEN:
                m_valueTxt.text = "¼±";
                break;
            case PROPERTYTYPE.PT_FRAUD:
                m_valueTxt.text = "»ç±â";
                break;
            case PROPERTYTYPE.PT_SECLUSION:
                m_valueTxt.text = "ÀºµÐ";
                break;
            case PROPERTYTYPE.PT_MADNESS:
                m_valueTxt.text = "±¤±â";
                break;
        }
        m_valueTxt.text += " + " + m_item.point.ToString();

        m_infoTxt.text = m_item.itemInfo;
    }
}
