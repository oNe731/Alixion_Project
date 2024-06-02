using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemIcon; // ������ ������
    public TMP_Text itemCountText; // ������ ���� �ؽ�Ʈ

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
