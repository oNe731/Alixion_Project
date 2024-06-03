using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �켱 ���� : �ĸ� > �� > ��� > ���� > ���� 
public enum PROPERTYTYPE { PT_RUIN, PT_ZEN, PT_FRAUD, PT_SECLUSION, PT_MADNESS, PT_END };

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    [SerializeField] private GameObject m_profilePanel;
    [SerializeField] private GameObject m_inventoryPanel;
    [SerializeField] private GameObject m_encyclopediaPanel;
    [SerializeField] private GameObject m_settingPanel;

    private int[] m_alienPoint;
    private Inventory m_inventory;

    public GameObject InventoryPanel => m_inventoryPanel;
    public Inventory Inventory => m_inventory;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);

            m_alienPoint = new int[(int)PROPERTYTYPE.PT_END];
            m_inventory = GetComponent<Inventory>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #region EVOLUTION
    // ���� ���� �� ������ ��� ���丮�� ������ ������ �־�����.
    // ����) �̸� ���� ���� ����
    // �̴ϰ����� ���� �������� ��´�. (���� �� �� ���� ���� ����)

    public void Update_Alien()
    {
        // ������ �κ��丮���� �����ؼ� ���δ�.
        // ���� ��ġ���� �����Ѵ�.
        // ������ ������ ������ ����ī�尡 ������ ������ �߰��ȴ�.


        // ���� ��ġ�� �Ѿ��ٸ� �Ѹ�� ��ȭ�ȴ�. 0 ~ 14 / 15 ~ 29
        // 30 ~ 44�� �Ѿ��ٸ� ���� ���� �Ϸ� �� ���� �߰�
        // �켱 ���� : �ĸ� > �� > ��� > ���� > ���� 


        // 1������ Ÿ�� ����Ʈ�� �����ϴ��� �˻�
        // 1������ Ÿ�Ը� �����ϸ�
    }

    public void UpdateStatusObject()
    {
        // ����Ʈ���� ����Ʈ�� ��� ����
        List<(string, int, int)> points = new List<(string, int, int)>
        {
            //("destroy", destroyPoints, 1),
            //("good", goodPoints, 2),
            //("cheat", cheatPoints, 3),
            //("seclusion", seclusionPoints, 4),
            //("chaos", chaosPoints, 5)
        };

        // ����Ʈ �켱������� ���� (���� ������ �켱������ ���� �������)
        points.Sort((a, b) =>
        {
            int comparison = b.Item2.CompareTo(a.Item2);
            return comparison == 0 ? a.Item3.CompareTo(b.Item3) : comparison;
        });

        // ���� ���� �� ���� ����Ʈ�� ����
        string highest1 = points[0].Item1;
        string highest2 = points[1].Item1;

        int objectIndex = GetObjectIndex(highest1, highest2);

        if (objectIndex != -1)
        {
            //if (currentActiveObject != null)
            //{
            //    currentActiveObject.SetActive(false);
            //}
            //currentActiveObject = statusObjects[objectIndex];
            //currentActiveObject.SetActive(true);
        }
    }

    // ���� ������Ʈ �ε����� �����ϴ� �Լ�
    private int GetObjectIndex(string highest1, string highest2)
    {
        int baseIndex = GetBaseIndex(highest1, highest2);
        if (baseIndex == -1)
        {
            return -1; // ���� �߻� ��
        }

        int totalPoints = 0;// destroyPoints + cheatPoints + goodPoints + seclusionPoints + chaosPoints;
        if (totalPoints >= 0 && totalPoints <= 14)
        {
            return baseIndex;
        }
        else if (totalPoints >= 15 && totalPoints <= 29)
        {
            return baseIndex + 15;
        }
        else if (totalPoints >= 30 && totalPoints <= 44)
        {
            return baseIndex + 30;
        }
        return -1; // ������ ����� ���
    }

    private int GetBaseIndex(string highest1, string highest2)
    {
        if (highest1 == "good" && highest2 == "good") return 0;
        if (highest1 == "good" && highest2 == "destroy") return 1;
        if (highest1 == "good" && highest2 == "cheat") return 2;
        if (highest1 == "good" && highest2 == "seclusion") return 3;
        if (highest1 == "good" && highest2 == "chaos") return 4;
        if (highest1 == "destroy" && highest2 == "destroy") return 5;
        if (highest1 == "destroy" && highest2 == "cheat") return 6;
        if (highest1 == "destroy" && highest2 == "seclusion") return 7;
        if (highest1 == "destroy" && highest2 == "chaos") return 8;
        if (highest1 == "cheat" && highest2 == "cheat") return 9;
        if (highest1 == "cheat" && highest2 == "seclusion") return 10;
        if (highest1 == "cheat" && highest2 == "chaos") return 11;
        if (highest1 == "seclusion" && highest2 == "seclusion") return 12;
        if (highest1 == "seclusion" && highest2 == "chaos") return 13;
        if (highest1 == "chaos" && highest2 == "chaos") return 14;
        return -1; // ���� �߻� ��
    }
    #endregion

    #region BUTTON
    public void Open_Profile()
    {
        if (m_profilePanel == null)
            return;

        m_profilePanel.SetActive(true);
    }

    public void Close_Profile()
    {
        if (m_profilePanel == null)
            return;

        m_profilePanel.SetActive(false);
    }

    public void Open_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_inventoryPanel.SetActive(true);
    }

    public void Close_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_inventoryPanel.SetActive(false);
    }

    public void Open_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        m_encyclopediaPanel.SetActive(true);
    }

    public void Close_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        m_encyclopediaPanel.SetActive(false);
    }

    public void Open_Settings()
    {
        if (m_settingPanel == null)
            return;

        m_settingPanel.SetActive(true);
    }

    public void Close_Settings()
    {
        if (m_settingPanel == null)
            return;

        m_settingPanel.SetActive(false);
    }

    public void Exit_Game()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
