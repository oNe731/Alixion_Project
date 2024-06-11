using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    
    private int m_slotCount = 15;
    private List<EncyclopediaSlot> m_slots = new List<EncyclopediaSlot>();

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/MainGame/Encyclopedia/EncyclopediaSlot");
        Transform parentTransform = transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).transform;

        // 도감 슬롯 생성
        for (int i = 0; i < m_slotCount; i++)
        {
            GameObject slot = Instantiate(prefab, parentTransform);

            EncyclopediaSlot script = slot.GetComponent<EncyclopediaSlot>();
            script.Encyclopedia = this;
            m_slots.Add(script);
        }
    }

    public void Add_Item(AlienData data)
    {
        // 중복 아이템 검사
        bool sameItem = false;
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                if (m_slots[i].Item.Type == data.Type)
                {
                    sameItem = true;
                    break;
                }
            }
        }

        if (sameItem == true)
            return;

        // 아이템 추가
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == true)
            {
                m_slots[i].Add_Item(data);
                m_slider.value += 1;
                break;
            }
        }
    }

    public bool Get_EmptyEncyclopedia()
    {
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                if (m_slots[i].EMPTY == false) // 도감에 존재
                {
                    return false;
                }
            }
        }

        return true;
    }
}
