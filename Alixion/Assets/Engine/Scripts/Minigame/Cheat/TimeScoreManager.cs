using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeScoreManager : MonoBehaviour
{
    public float timeLimit = 30f; // 30초으로 설정
    public int scoreToChange1Scene = 10; // 점수가 10이 넘으면 패널1 활성화
    public int scoreToChange2Scene = 20; // 점수가 20이 넘으면 패널2 활성화
    public int scoreToChange3Scene = 30; // 점수가 30이 넘으면 패널3 활성화

    public TMP_Text timeText;
    public TMP_Text scoreText;

    public GameObject panel1; // 패널1 UI 요소
    public GameObject panel2; // 패널2 UI 요소
    public GameObject panel3; // 패널3 UI 요소
    public GameObject gameOverPanel; // 게임 오버 패널
    public Button transitionButton1; // 씬 전환 버튼 1
    public Button transitionButton2; // 씬 전환 버튼 2
    public Button transitionButton3; // 씬 전환 버튼 3

    public Sprite itemSprite1; // 클리어 1에서 추가할 아이템 스프라이트
    public Sprite itemSprite2; // 클리어 2에서 추가할 아이템 스프라이트
    public Sprite itemSprite3; // 클리어 3에서 추가할 아이템 스프라이트

    public int score = 0;
    private float currentTime;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;

        // 모든 패널 비활성화
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        gameOverPanel.SetActive(false);
        transitionButton1.gameObject.SetActive(false); // 버튼 비활성화
        transitionButton2.gameObject.SetActive(false); // 버튼 비활성화
        transitionButton3.gameObject.SetActive(false); // 버튼 비활성화


    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        // 시간 관리
        currentTime -= Time.deltaTime;
        timeText.text = "Time: " + currentTime.ToString("F0");

        if (currentTime <= 0)
        {
            // 시간이 다 되면 점수 관리
            EndGame();
        }
    }

    void EndGame()
    {
        isGameOver = true; // 게임 종료 상태 설정

        // 최종 점수 업데이트
        scoreText.text = "Score: " + score;

        if (score >= scoreToChange3Scene)
        {
            // 점수가 30 이상이면 패널3 활성화
            panel3.SetActive(true);
            AddItemToInventory(itemSprite3, 1); // 클리어 3 아이템 1개 추가
            AddItemToInventory(itemSprite2, 1); // 클리어 2 아이템 1개 추가
            AddItemToInventory(itemSprite1, 1); // 클리어 1 아이템 1개 추가
            transitionButton3.gameObject.SetActive(true); // 버튼 활성화
        }
        else if (score >= scoreToChange2Scene)
        {
            // 점수가 20 이상이면 패널2 활성화
            panel2.SetActive(true);
            AddItemToInventory(itemSprite2, 1); // 클리어 2 아이템 1개 추가
            AddItemToInventory(itemSprite1, 1); // 클리어 1 아이템 1개 추가
            transitionButton2.gameObject.SetActive(true); // 버튼 활성화
        }
        else if (score >= scoreToChange1Scene)
        {
            // 점수가 10 이상이면 패널1 활성화
            panel1.SetActive(true);
            AddItemToInventory(itemSprite1, 1); // 클리어 1 아이템 1개 추가
            transitionButton1.gameObject.SetActive(true); // 버튼 활성화
        }
        else
        {
            // 점수가 기준에 못 미치면 게임 오버 패널 활성화
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
        scoreText.text = "Score: " + score; // 점수 텍스트 업데이트
    }

    public void OnTransitionButtonClick()
    {
        SceneManager.LoadScene("Game"); // 전환할 씬 이름을 넣으세요
    }
}
