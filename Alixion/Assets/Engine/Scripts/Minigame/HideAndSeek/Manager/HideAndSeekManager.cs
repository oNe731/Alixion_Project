using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HideAndSeekManager : LevelManager
{
    private static HideAndSeekManager m_instance = null;
    public static HideAndSeekManager Instance => m_instance;

    [SerializeField] private TMP_Text m_timerTxt;
    [SerializeField] private Slider m_barSlider;
    [SerializeField] private HideAndSeekHeart m_Heart;
    [SerializeField] private GameObject m_finishPanel;
    [SerializeField] private GameObject m_retryButton;
    [SerializeField] private GameObject m_homeButton;
    [SerializeField] private GameObject m_methodPanel;

    private GameObject m_player;
    private bool m_gameStop = false;
    private bool m_monitor  = false;

    private float m_timer = 60f;
    private float m_speed = 1f;

    private bool  m_reversal = false;
    private float m_reversalTime = 0f;

    private float m_itemTime = 0;
    private float m_itemCreate = 0;
    private float m_itemCreateMin = 5;
    private float m_itemCreateMax = 10;
    private int   m_beforItemIndex = -1;

    private GameObject m_goalFlag = null;

    public bool Monitor
    {
        get => m_monitor;
        set => m_monitor = value;
    }
    public bool GameStop
    {
       get => m_gameStop;
       set => m_gameStop = value;
    }
    public float Speed
    {
        get => m_speed;
        set => m_speed = value;
    }
    public bool Reversal
    {
        get => m_reversal;
        set => m_reversal = value;
    }

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
        else
            Destroy(this.gameObject);

        Screen.orientation = ScreenOrientation.LandscapeRight;
        Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

        m_player = GameObject.FindGameObjectWithTag("Player");
        m_itemCreate = Random.Range(m_itemCreateMin, m_itemCreateMax);
    }

    private void Update()
    {
        if (GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
            return;

        m_timer -= Time.deltaTime;
        if (m_timer <= 0.8f)
        {
            m_timerTxt.text = "00:00";
            Finish_Game(FinishPanel.FinishType.FT_FAIL);
        }
        else
        {
            m_timerTxt.text = string.Format("{0:00}:{1:00}",
                Mathf.FloorToInt(m_timer / 60f),
                Mathf.FloorToInt(m_timer % 60f));

            if (m_reversal)
            {
                m_reversalTime += Time.deltaTime;
                if (m_reversalTime > 10f)
                {
                    m_reversal = false;
                    m_reversalTime = 0f;
                    m_player.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }

    public override void Start_Game()
    {
        GameManager.Instance.IsMiniGame = true;

        m_player.GetComponent<Animator>().StopPlayback(); // 애니메이션 재생
        Camera.main.GetComponent<AudioSource>().Play();
    }

    public void Scroll_Game(float touchDir)
    {
        if (GameManager.Instance.IsMiniGame == false || m_monitor == true || GameManager.Instance.Pause == true) // 게임 시작 여부/ 감시자 효과 / 일시정지
            return;

        if (m_gameStop == true && touchDir == 1f) // 판넬 효과
            return;

        GameObject[] gameObject_Far = GameObject.FindGameObjectsWithTag("Far");
        foreach (GameObject obj in gameObject_Far)
        {
            obj.transform.position -= new Vector3(2.5f, 0f, 0f) * m_speed * Time.deltaTime * touchDir;
        }

        GameObject[] gameObject_Middle = GameObject.FindGameObjectsWithTag("Middle");
        foreach (GameObject obj in gameObject_Middle)
        {
            obj.transform.position -= new Vector3(3f, 0f, 0f) * m_speed * Time.deltaTime * touchDir;
        }

        GameObject[] gameObject_Near = GameObject.FindGameObjectsWithTag("Near");
        foreach (GameObject obj in gameObject_Near)
        {
            obj.transform.position -= new Vector3(3.5f, 0f, 0f) * m_speed * Time.deltaTime * touchDir;
        }

        GameObject[] gameObject_Front = GameObject.FindGameObjectsWithTag("Front");
        foreach (GameObject obj in gameObject_Front)
        {
            obj.transform.position -= new Vector3(4f, 0f, 0f) * m_speed * Time.deltaTime * touchDir;
        }

        // 스크롤바 증가
        m_barSlider.value += 1f * m_speed * Time.deltaTime * touchDir;
        if(m_barSlider.value >= m_barSlider.maxValue - 3f)
        {
            if(m_goalFlag == null)
            {
                m_goalFlag = Instantiate(Resources.Load<GameObject>("Prefabs/MiniGame/Seclusion_Game/Object/GoalFlag"));
                m_goalFlag.GetComponent<Transform>().position = new Vector3(13.3f, -3.2f, 0f);
            }
        }
        else if(m_barSlider.value < m_barSlider.maxValue - 3f)
        {
            if (m_goalFlag != null)
            {
                Destroy(m_goalFlag);
            }
        }

        // 게임 종료 판별
        if (m_barSlider.value == m_barSlider.maxValue)
        {
            Finish_Game(FinishPanel.FinishType.FT_CLEAR);
        }
        else
        {
            if(touchDir == 1f) // 전진시에만 생성
                Crate_Item();
        }
    }

    private void Finish_Game(FinishPanel.FinishType finishType)
    {
        GameManager.Instance.IsMiniGame = false;
        m_player.GetComponent<Animator>().StartPlayback();

        m_finishPanel.SetActive(true);
        if (finishType == FinishPanel.FinishType.FT_CLEAR)
        {
            m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_CLEAR);
            if (m_timer >= 10f)
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(3, "UI_Item_Seclusion1", "UI_Item_Seclusion2", "UI_Item_Seclusion3");
            else if (m_timer >= 5f)
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(2, "UI_Item_Seclusion1", "UI_Item_Seclusion2");
            else
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(1, "UI_Item_Seclusion1");
        }
        else if(finishType == FinishPanel.FinishType.FT_FAIL)
            m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_FAIL);
    }

    public void Update_Heart(int heartCount)
    {
        m_Heart.Update_Heart(heartCount);
        if (heartCount == 0)
        {
            // 게임 실패
            Finish_Game(FinishPanel.FinishType.FT_FAIL);
        }
    }

    private void Crate_Item()
    {
        m_itemTime += Time.deltaTime;
        if(m_itemTime > m_itemCreate)
        {
            m_itemTime   = 0f;
            m_itemCreate = Random.Range(m_itemCreateMin, m_itemCreateMax);

            while (true)
            {
                int random = Random.Range(0, 3);
                if (m_beforItemIndex != random)
                {
                    m_beforItemIndex = random;
                    break;
                }
            }

            GameObject item = null;
            switch (m_beforItemIndex)
            {
                case 0:
                    item = Instantiate(Resources.Load<GameObject>("Prefabs/MiniGame/Seclusion_Game/Object/Puddle"));
                    break;
                case 1:
                    item = Instantiate(Resources.Load<GameObject>("Prefabs/MiniGame/Seclusion_Game/Object/Bottle"));
                    break;
                case 2:
                    item = Instantiate(Resources.Load<GameObject>("Prefabs/MiniGame/Seclusion_Game/Object/Sign"));
                    break;
            }
            item.GetComponent<Transform>().position = new Vector3(15f, -3.5f, 0f);
        }
    }

    public void Button_MethodPanel()
    {
        GameManager.Instance.Pause = true;
        m_methodPanel.SetActive(true);
    }
}
