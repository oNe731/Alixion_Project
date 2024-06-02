using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject[] statusObjects; // �� 45���� ���� ������Ʈ
    private GameObject currentActiveObject;

    public GameObject miniGamePanel;
    public GameObject miniGameButton;

    public GameObject detailedProfilePanel;

    public GameObject inventoryPanel;
    public GameObject settingsPanel;
    public GameObject codexPanel;

    // Ư�� ����Ʈ
    private int destroyPoints = 0;
    private int cheatPoints = 0;
    private int goodPoints = 0;
    private int seclusionPoints = 0;
    private int chaosPoints = 0;

    void Start()
    {
        currentActiveObject = null;
    }

    // ������ ���� �Լ�
    public void ShowProfile()
    {
        detailedProfilePanel.SetActive(true);
    }

    public void HideProfile()
    {
        detailedProfilePanel.SetActive(false);
    }

    // �̴ϰ��� �г� ���� �Լ�
    public void HideMiniGame()
    {
        miniGamePanel.SetActive(false);
    }

    public void OpenMiniGamePanel()
    {
        miniGamePanel.SetActive(true);
    }

    // ���� ������Ʈ ������Ʈ �Լ�
    public void UpdateStatusObject()
    {
        // ����Ʈ���� ����Ʈ�� ��� ����
        List<(string, int, int)> points = new List<(string, int, int)>
        {
            ("destroy", destroyPoints, 1),
            ("good", goodPoints, 2),
            ("cheat", cheatPoints, 3),
            ("seclusion", seclusionPoints, 4),
            ("chaos", chaosPoints, 5)
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
            if (currentActiveObject != null)
            {
                currentActiveObject.SetActive(false);
            }
            currentActiveObject = statusObjects[objectIndex];
            currentActiveObject.SetActive(true);
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

        int totalPoints = destroyPoints + cheatPoints + goodPoints + seclusionPoints + chaosPoints;
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

    // Ư�� ����Ʈ ���� �Լ�
    public void AddDestroyPoints(int points)
    {
        destroyPoints += points;
        UpdateStatusObject();
    }

    public void AddCheatPoints(int points)
    {
        cheatPoints += points;
        UpdateStatusObject();
    }

    public void AddGoodPoints(int points)
    {
        goodPoints += points;
        UpdateStatusObject();
    }

    public void AddSeclusionPoints(int points)
    {
        seclusionPoints += points;
        UpdateStatusObject();
    }

    public void AddChaosPoints(int points)
    {
        chaosPoints += points;
        UpdateStatusObject();
    }

    // �� ��ȯ �Լ� 5��
    public void LoadSceneGood()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void LoadSceneDestory()
    {
        SceneManager.LoadScene("Destory");
    }

    public void LoadSceneCheat()
    {
        SceneManager.LoadScene("Cheat");
    }

    public void LoadSceneSeclusion()
    {
        SceneManager.LoadScene("Scene4");
    }

    public void LoadSceneChaos()
    {
        SceneManager.LoadScene("Scene5");
    }

    // �κ��丮 �г�
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }

    // ���� �г�
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    // ���� �г�
    public void ShowCodex()
    {
        codexPanel.SetActive(true);
    }

    public void HideCodex()
    {
        codexPanel.SetActive(false);
    }
}
