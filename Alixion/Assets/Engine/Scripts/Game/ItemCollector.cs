using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public InventoryManager inventoryManager; // �κ��丮 �Ŵ���

    // ���÷� �浹 �� �������� ȹ���ϴ� ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Sprite itemSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            int itemCount = 1; // ȹ���� �������� ����
            inventoryManager.AddItem(itemSprite, itemCount);
            Destroy(collision.gameObject); // ������ ������Ʈ ����
        }
    }
}
