using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // �̱��� �ν��Ͻ�

    public GameObject inventoryPanel; // �κ��丮 �г�
    public GameObject slotPrefab; // ���� ������
    public int slotCount = 15; // ���� ����
    private List<InventorySlot> slots = new List<InventorySlot>(); // ���� ����Ʈ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �κ��丮 �Ŵ����� ���ŵ��� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ���� ����
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
