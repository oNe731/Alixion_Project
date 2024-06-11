using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaSlot : MonoBehaviour
{
    private bool m_empty = true;
    private AlienData m_item = null;
    private Encyclopedia m_encyclopedia = null;

    public bool EMPTY => m_empty;
    public AlienData Item => m_item;
    public Encyclopedia Encyclopedia { set => m_encyclopedia = value; }

    public void Add_Item(AlienData data)
    {
        m_item = data;

        Sprite[] sprite = null;
        string basicPath = "Sprites/MainGame/Alien/";

        switch (m_item.Type)
        {
            case ALIENTYPE.AT_RUIN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Ruin/Ruin_3");
                break;

            case ALIENTYPE.AT_ZEN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Zen/Zen_3");
                break;

            case ALIENTYPE.AT_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Fraud/Fraud_3");
                break;

            case ALIENTYPE.AT_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Seclusion/Seclusion_3");
                break;

            case ALIENTYPE.AT_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Madness/Madness_3");
                break;

            case ALIENTYPE.AT_ZEN_RUIN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Ruin/Zen+Ruin_3");
                break;

            case ALIENTYPE.AT_ZEN_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Fraud/Zen+Fraud_3");
                break;

            case ALIENTYPE.AT_ZEN_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Seclusion/Zen+Seclusion_3");
                break;

            case ALIENTYPE.AT_ZEN_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Madness/Zen+Madness_3");
                break;

            case ALIENTYPE.AT_RUIN_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Fraud/Ruin+Fraud_3");
                break;

            case ALIENTYPE.AT_RUIN_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Seclusion/Ruin+Seclusion_3");
                break;

            case ALIENTYPE.AT_RUIN_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Madness/Ruin+Madness_3");
                break;

            case ALIENTYPE.AT_FRAUD_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Fraud+Seclusion/Fraud+Seclusion_3");
                break;

            case ALIENTYPE.AT_FRAUD_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Fraud+Madness/Fraud+Madness_3");
                break;

            case ALIENTYPE.AT_SECLUSION_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Seclusion+Madness/Seclusion+Madness_3");
                break;
        }

        gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = sprite[0];
        gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700f, 700f);

        m_empty = false;
    }

    public void Click_Slot()
    {
        if (m_empty == true)
            return;

        // 카드 생성
        GameObject cardObject = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Encyclopedia/Panel_Encyclopedia_Popup"), GameManager.Instance.EncyclopediaPanel.transform);
        cardObject.GetComponent<EncyclopediaCard>().Set_Card(m_item, false);
    }
}
