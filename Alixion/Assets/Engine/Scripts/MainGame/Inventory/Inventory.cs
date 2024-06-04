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

        // �κ��丮 ���� ����
        for(int i = 0; i < m_slotCount; i++) {
            GameObject slot = Instantiate(prefab, parentTransform);

            InvenSlot script = slot.GetComponent<InvenSlot>();
            script.Inventory = this;
            m_slots.Add(script);
        }
    }

    public void Add_Item(string itemName)
    {
        // �ߺ� ������ �˻�
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

        // ������ �߰�
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
            // ������ ����
            m_selectIcon = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/InvenSlotSelect"));
        }

        // ������ �̵�
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

        // �� ������ ������ ������ ���Ը� �߰�
        foreach (InvenSlot slot in m_slots) { if (!slot.EMPTY) { sortedSlots.Add(slot); } }
        // ������ ������ �� �������� ä���
        for (int i = sortedSlots.Count; i < m_slotCount; i++) { sortedSlots.Add(null); }

        // ���ĵ� ���� ����Ʈ�� �ٽ� ����
        for (int i = 0; i < m_slotCount; i++)
        {
            if (sortedSlots[i] != null && sortedSlots[i].EMPTY == false) { m_slots[i].Add_Item(sortedSlots[i].Item.objectName); }
            else { m_slots[i].Reset_Slot(); }
        }
    }
}
