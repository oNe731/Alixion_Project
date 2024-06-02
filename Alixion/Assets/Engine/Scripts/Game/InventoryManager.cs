using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // 싱글톤 인스턴스

    public GameObject inventoryPanel; // 인벤토리 패널
    public GameObject slotPrefab; // 슬롯 프리팹
    public int slotCount = 15; // 슬롯 개수
    private List<InventorySlot> slots = new List<InventorySlot>(); // 슬롯 리스트

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 인벤토리 매니저가 제거되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
            InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
            slots.Add(slotComponent);
        }
    }

    public void AddItem(Sprite itemSprite, int itemCount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(itemSprite, itemCount);
                return;
            }
        }
    }
}
