using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public void Add_Item(string itemName, int Count = 1)
    {
        Reset_Slot();

        m_uIItem = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + itemName), gameObject.transform);
        if (m_uIItem == null)
            return;

        m_uIItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        m_uIItem.transform.localScale = new Vector3(2f, 2f, 1f);
        m_item = m_uIItem.GetComponent<ItemData>();

        m_item.count = Count;
        if (m_item.count > 1)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
        }

        m_empty = false;
    }

    public void Add_Item(int count)
    {
        m_item.count += count;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
    }

    public void Use_Item()
    {
        m_item.count -= 1;
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = m_item.count.ToString();
        if (m_item.count == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else if(m_item.count <= 0)
        {
            Reset_Slot();
            inventory.Sort_Inventory();
        }
    }

    public void Reset_Slot()
    {
        transform.GetChild(0).gameObject.SetActive(false);

        m_empty = true;
        m_item = null;

        if (m_uIItem != null) { Destroy(m_uIItem); }
    }

    public void Click_Slot()
    {
        inventory.Selct_Slot(this);
        GameManager.Instance.Open_InventoryItem(m_item);
    }
}
