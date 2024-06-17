namespace Zen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ZenManager : LevelManager
    {
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private Slider          m_timerSlider;
        [SerializeField] private ObjectSpawner   m_objectSpawner;
        [SerializeField] private GameObject      m_finishPanel;
        [SerializeField] private float m_maxTime = 30;

        private float m_currentTime = 0;
        private int m_maxScore = 20;
        private int m_midScore = 15;
        private int m_minScore = 10;
        private int m_score = 0;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

            m_currentTime = m_maxTime;
            Update_ScoreText();
            Update_SliderValue();
        }

        public override void Start_Game()
        {
            m_objectSpawner.Create_Object();
            Camera.main.gameObject.GetComponent<AudioSource>().Play();
            GameManager.Instance.IsMiniGame = true;
        }

        private void Update()
        {
            if (GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
                return;

            m_currentTime -= Time.deltaTime;
            Update_SliderValue();

            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Trash");
            if (m_currentTime <= 0f || objectsWithTag.Length == 0)
                Over_Game();
        }

        public void Add_TimeCount(float timeDec)
        {
            m_currentTime += timeDec;
            if (m_currentTime > m_maxTime)
                m_currentTime = m_maxTime;
        }

        public void Update_SliderValue()
        {
            float sliderValue = m_currentTime / m_maxTime;  // 슬라이더 값 계산
            m_timerSlider.value = sliderValue;  // 슬라이더 값 업데이트
        }

        public void Add_Score(int amount)
        {
            m_score += amount;
            Update_ScoreText();
        }

        private void Update_ScoreText()
        {
            m_scoreText.text = "점수: " + m_score;
        }

        public void Over_Game()
        {
            GameManager.Instance.IsMiniGame = false;

            m_finishPanel.SetActive(true);
            if (m_score >= m_minScore)
            {
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_CLEAR);
                if (m_score >= m_maxScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(3, "UI_Item_Zen1", "UI_Item_Zen2", "UI_Item_Zen3");
                else if (m_score >= m_midScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(2, "UI_Item_Zen1", "UI_Item_Zen2");
                else if (m_score >= m_minScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(1, "UI_Item_Zen1");
            }
            else
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_FAIL);
        }
    }
}