using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon; // 아이템 아이콘
    public TMP_Text itemCountText; // 아이템 개수 텍스트

    private int itemCount;

    public void SetItem(Sprite itemSprite, int count)
    {
        itemIcon.sprite = itemSprite;
        itemIcon.enabled = true;
        itemCount = count;
        itemCountText.text = itemCount.ToString();
        itemCountText.enabled = true;
    }

    public bool HasItem()
    {
        return itemIcon.enabled;
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemCount = 0;
        itemCountText.text = "";
        itemCountText.enabled = false;
    }
}
