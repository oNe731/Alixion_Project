using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeScoreManager : MonoBehaviour
{
    public float timeLimit = 30f; // 30������ ����
    public int scoreToChange1Scene = 10; // ������ 10�� ������ �г�1 Ȱ��ȭ
    public int scoreToChange2Scene = 20; // ������ 20�� ������ �г�2 Ȱ��ȭ
    public int scoreToChange3Scene = 30; // ������ 30�� ������ �г�3 Ȱ��ȭ

    public TMP_Text timeText;
    public TMP_Text scoreText;

    public GameObject panel1; // �г�1 UI ���
    public GameObject panel2; // �г�2 UI ���
    public GameObject panel3; // �г�3 UI ���
    public GameObject gameOverPanel; // ���� ���� �г�
    public Button transitionButton1; // �� ��ȯ ��ư 1
    public Button transitionButton2; // �� ��ȯ ��ư 2
    public Button transitionButton3; // �� ��ȯ ��ư 3

    public Sprite itemSprite1; // Ŭ���� 1���� �߰��� ������ ��������Ʈ
    public Sprite itemSprite2; // Ŭ���� 2���� �߰��� ������ ��������Ʈ
    public Sprite itemSprite3; // Ŭ���� 3���� �߰��� ������ ��������Ʈ

    public int score = 0;
    private float currentTime;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;

        // ��� �г� ��Ȱ��ȭ
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        gameOverPanel.SetActive(false);
        transitionButton1.gameObject.SetActive(false); // ��ư ��Ȱ��ȭ
        transitionButton2.gameObject.SetActive(false); // ��ư ��Ȱ��ȭ
        transitionButton3.gameObject.SetActive(false); // ��ư ��Ȱ��ȭ


    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        // �ð� ����
        currentTime -= Time.deltaTime;
        timeText.text = "Time: " + currentTime.ToString("F0");

        if (currentTime <= 0)
        {
            // �ð��� �� �Ǹ� ���� ����
            EndGame();
        }
    }

    void EndGame()
    {
        isGameOver = true; // ���� ���� ���� ����

        // ���� ���� ������Ʈ
        scoreText.text = "Score: " + score;

        if (score >= scoreToChange3Scene)
        {
            // ������ 30 �̻��̸� �г�3 Ȱ��ȭ
            panel3.SetActive(true);
            AddItemToInventory(itemSprite3, 1); // Ŭ���� 3 ������ 1�� �߰�
            AddItemToInventory(itemSprite2, 1); // Ŭ���� 2 ������ 1�� �߰�
            AddItemToInventory(itemSprite1, 1); // Ŭ���� 1 ������ 1�� �߰�
            transitionButton3.gameObject.SetActive(true); // ��ư Ȱ��ȭ
        }
        else if (score >= scoreToChange2Scene)
        {
            // ������ 20 �̻��̸� �г�2 Ȱ��ȭ
            panel2.SetActive(true);
            AddItemToInventory(itemSprite2, 1); // Ŭ���� 2 ������ 1�� �߰�
            AddItemToInventory(itemSprite1, 1); // Ŭ���� 1 ������ 1�� �߰�
            transitionButton2.gameObject.SetActive(true); // ��ư Ȱ��ȭ
        }
        else if (score >= scoreToChange1Scene)
        {
            // ������ 10 �̻��̸� �г�1 Ȱ��ȭ
            panel1.SetActive(true);
            AddItemToInventory(itemSprite1, 1); // Ŭ���� 1 ������ 1�� �߰�
            transitionButton1.gameObject.SetActive(true); // ��ư Ȱ��ȭ
        }
        else
        {
            // ������ ���ؿ� �� ��ġ�� ���� ���� �г� Ȱ��ȭ
            gameOverPanel.SetActive(true);
        }
    }

    void AddItemToInventory(Sprite itemSprite, int itemCount)
    {
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(itemSprite, itemCount);
        }
        else
        {
            Debug.LogError("InventoryManager instance not found");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score; // ���� �ؽ�Ʈ ������Ʈ
    }

    public void OnTransitionButtonClick()
    {
        SceneManager.LoadScene("Game"); // ��ȯ�� �� �̸��� ��������
    }
}
