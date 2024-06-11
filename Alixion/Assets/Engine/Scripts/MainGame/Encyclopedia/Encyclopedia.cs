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

        // ���� ���� ����
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
        // �ߺ� ������ �˻�
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

        // ������ �߰�
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
                if (m_slots[i].EMPTY == false) // ������ ����
                {
                    return false;
                }
            }
        }

        return true;
    }
}
