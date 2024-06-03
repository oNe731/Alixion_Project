using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot : MonoBehaviour
{
    private bool m_empty  = true;
    private ItemData m_item = null;
    public bool EMPTY => m_empty;
    public ItemData Item => m_item;

    public void Add_Item(string itemName)
    {
        GameObject UIitem = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + itemName), gameObject.transform);

        if (UIitem == null)
            return;

        UIitem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        UIitem.transform.localScale = new Vector3(2f, 2f, 1f);
        m_item = UIitem.GetComponent<ItemData>();

        m_empty = false;
    }
}
