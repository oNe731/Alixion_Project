namespace Madness
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class MadnessManager : LevelManager
    {
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private Slider          m_timerSlider;
        [SerializeField] private GameObject m_leftUI;
        [SerializeField] private GameObject m_rightUI;
        [SerializeField] private GameObject m_jumpUI;
        [SerializeField] private GameObject m_stealUI;
        [SerializeField] private GameObject m_finishPanel;
        [SerializeField] private PlayerController m_playerController;
        [SerializeField] private GameObject[] m_enemy;

        private float m_maxTime = 30;
        private float m_currentTime = 0;
        private int m_maxScore = 20;
        private int m_midScore = 15;
        private int m_minScore = 10;
        private int m_score = 0;

        public int Score
        {
            get => m_score;
            set => m_score = value;
        }

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
            Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

            m_currentTime = m_maxTime;
            Update_ScoreText();
            Update_SliderValue();
        }

        public override void Start_Game()
        {
            Camera.main.gameObject.GetComponent<AudioSource>().Play();
            GameManager.Instance.IsMiniGame = true;

            m_jumpUI.GetComponent<Button>().interactable = true;
            m_stealUI.GetComponent<Button>().interactable = true;
            m_rightUI.GetComponent<Button>().interactable = true;
            m_leftUI.GetComponent<Button>().interactable = true;

            for(int i = 0; i < m_enemy.Length; ++i)
                m_enemy[i].GetComponent<Animator>().StopPlayback();
        }

        void Update()
        {
            if (GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
                return;

            Update_ScoreText();
            Update_TimeCount();
        }

        private void Update_ScoreText()
        {
            m_scoreText.text = "Á¡¼ö: " + m_score;
        }

        public void Update_TimeCount()
        {
            m_currentTime -= Time.deltaTime;
            if (m_currentTime < 0f)
            {
                m_currentTime = 0f;
                Over_Game();
            }

            Update_SliderValue();
        }

        public void Update_SliderValue()
        {
            float sliderValue = m_currentTime / m_maxTime;
            m_timerSlider.value = sliderValue;
        }

        public void Over_Game()
        {
            m_jumpUI.GetComponent<Button>().interactable = false;
            m_stealUI.GetComponent<Button>().interactable = false;
            m_rightUI.GetComponent<Button>().interactable = false;
            m_leftUI.GetComponent<Button>().interactable = false;

            GameManager.Instance.IsMiniGame = false;

            m_finishPanel.SetActive(true);
            if (m_score >= m_minScore)
            {
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_CLEAR);
                if (m_score >= m_maxScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(3, "UI_Item_Madness1", "UI_Item_Madness2", "UI_Item_Madness3");
                else if (m_score >= m_midScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(2, "UI_Item_Madness1", "UI_Item_Madness2");
                else if (m_score >= m_minScore)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(1, "UI_Item_Madness1");
            }
            else
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_FAIL);
        }
    }
}