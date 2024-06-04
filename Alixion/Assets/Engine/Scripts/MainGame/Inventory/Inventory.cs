using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int m_slotCount = 15;
    private List<InvenSlot> m_slots = new List<InvenSlot>();

    private InvenSlot m_selctSlot = null;
    private GameObject m_selectIcon = null;
    public InvenSlot SelctSlot { set => m_selctSlot = value; }

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/MainGame/Inventory/InvenSlot");
        Transform parentTransform = transform.GetChild(0).GetChild(1).GetChild(3).transform;

        // 인벤토리 슬롯 생성
        for(int i = 0; i < m_slotCount; i++) {
            GameObject slot = Instantiate(prefab, parentTransform);

            InvenSlot script = slot.GetComponent<InvenSlot>();
            script.Inventory = this;
            m_slots.Add(script);
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

    public void Selct_Slot(InvenSlot selctSlot)
    {
        m_selctSlot = selctSlot;
        if(m_selectIcon == null)
        {
            // 아이콘 생성
            m_selectIcon = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/InvenSlotSelect"));
        }

        // 포지션 이동
        m_selectIcon.transform.SetParent(m_selctSlot.transform);
        m_selectIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        m_selectIcon.transform.localScale = new Vector3(2.2f, 2.2f, 1f);

        m_selectIcon.GetComponent<InvenSlotSelect>().Start_Coroutine();
    }

    public void Use_Item()
    {
        if (m_selctSlot == null || m_selctSlot.EMPTY == true || GameManager.Instance == null)
            return;

        GameManager.Instance.Add_Point(m_selctSlot.Item.propertyType, m_selctSlot.Item.point);
        m_selctSlot.Reset_Slot();
        Sort_Inventory();
    }

    private void Sort_Inventory()
    {
        List<InvenSlot> sortedSlots = new List<InvenSlot>();

        // 빈 슬롯을 제외한 아이템 슬롯만 추가
        foreach (InvenSlot slot in m_slots) { if (!slot.EMPTY) { sortedSlots.Add(slot); } }
        // 나머지 슬롯을 빈 슬롯으로 채우기
        for (int i = sortedSlots.Count; i < m_slotCount; i++) { sortedSlots.Add(null); }

        // 정렬된 슬롯 리스트를 다시 설정
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false) { m_slots[i].Add_Item(sortedSlots[i].Item.objectName); }
            else { m_slots[i].Reset_Slot(); }
        }
    }
}
