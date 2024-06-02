using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public InventoryManager inventoryManager; // 인벤토리 매니저

    // 예시로 충돌 시 아이템을 획득하는 방식
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Sprite itemSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            int itemCount = 1; // 획득한 아이템의 개수
            inventoryManager.AddItem(itemSprite, itemCount);
            Destroy(collision.gameObject); // 아이템 오브젝트 삭제
        }
    }
}
