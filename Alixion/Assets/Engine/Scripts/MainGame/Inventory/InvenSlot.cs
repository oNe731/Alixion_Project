using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot : MonoBehaviour
{
    private bool m_empty  = true;
    private GameObject m_uIItem = null;
    private ItemData   m_item = null;
    private Inventory  inventory = null;

    public bool EMPTY => m_empty;
    public ItemData Item => m_item;
    public Inventory Inventory { set => inventory = value; }

    public void Add_Item(string itemName)
    {
        Reset_Slot();

        m_uIItem = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + itemName), gameObject.transform);
        if (m_uIItem == null)
            return;

        m_uIItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        m_uIItem.transform.localScale = new Vector3(2f, 2f, 1f);
        m_item = m_uIItem.GetComponent<ItemData>();

        m_empty = false;
    }
    
    public void Reset_Slot()
    {
        m_empty = true;
        m_item = null;

        if (m_uIItem != null) { Destroy(m_uIItem); }
    }

    public void Click_Slot()
    {
        inventory.Selct_Slot(this);
        //GameManager.Instance.Open_InventoryItem(m_item);
    }
}
