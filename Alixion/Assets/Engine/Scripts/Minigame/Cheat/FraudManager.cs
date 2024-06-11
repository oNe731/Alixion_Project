namespace Fraud
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine.SceneManagement;

    public class FraudManager : LevelManager
    {
        [SerializeField] private TMP_Text m_timeText;
        [SerializeField] private TMP_Text m_scoreText;
        [SerializeField] private GameObject m_finishPanel;

        private float m_timeLimit = 30f;
        private int m_scoreToChange1Scene = 5;
        private int m_scoreToChange2Scene = 12;
        private int m_scoreToChange3Scene = 20;

        private int   m_score = 0;
        private float m_currentTime = 0;

        private AudioSource m_audioSource;

        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

            m_audioSource = GetComponent<AudioSource>();
            m_scoreText.text = "Score: " + m_score;
        }

        public override void Start_Game()
        {
            m_currentTime = m_timeLimit;

            GetComponent<BlockSpawner>().Spawn_Blocks(20);
            Camera.main.gameObject.GetComponent<AudioSource>().Play();
            GameManager.Instance.IsMiniGame = true;
        }

        void Update()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            m_currentTime -= Time.deltaTime;
            m_timeText.text = "Time: " + m_currentTime.ToString("F0");

            if (m_currentTime > 0)
            {
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Block");
                if (objectsWithTag.Length == 0)
                    GetComponent<BlockSpawner>().Spawn_Blocks(10);
            }
            else
                Over_Game();
        }

        void Over_Game()
        {
            GameManager.Instance.IsMiniGame = false;

            m_finishPanel.SetActive(true);
            if (m_score >= m_scoreToChange1Scene)
            {
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_CLEAR);
                if (m_score >= m_scoreToChange3Scene)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(3, "UI_Item_Seclusion1", "UI_Item_Seclusion2", "UI_Item_Seclusion3");
                else if (m_score >= m_scoreToChange2Scene)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(2, "UI_Item_Seclusion1", "UI_Item_Seclusion2");
                else if (m_score >= m_scoreToChange1Scene)
                    m_finishPanel.GetComponent<FinishPanel>().Create_Item(1, "UI_Item_Seclusion1");
            }
            else
                m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_FAIL);
        }

        public void AddScore(int amount)
        {
            m_score += amount;
            m_scoreText.text = "Score: " + m_score;
        }

        public void Play_Sound(string path)
        {
            m_audioSource.clip = Resources.Load<AudioClip>(path);
            m_audioSource.Play();
        }
    }
}

