using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int m_slotCount = 15;
    private List<InvenSlot> m_slots = new List<InvenSlot>();

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/MainGame/Inventory/InvenSlot");
        Transform parentTransform = transform.GetChild(0).GetChild(1).GetChild(3).transform;

        // 인벤토리 슬롯 생성
        for(int i = 0; i < m_slotCount; i++) {
            GameObject slot = Instantiate(prefab, parentTransform);
            m_slots.Add(slot.GetComponent<InvenSlot>());
        }
    }

    public void Add_Item(string itemName)
    {
        // 중복 아이템 검사
        bool sameItem = false;
        for (int i = 0; i < m_slotCount; i++)
        {
            if (m_slots[i].EMPTY == false)
            {
                if(m_slots[i].Item.objectName == itemName)
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
                m_slots[i].Add_Item(itemName);
                break;
            }
        }
    }
}
