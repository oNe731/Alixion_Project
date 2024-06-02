using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HideAndSeekManager : MonoBehaviour
{
    public enum LEVEL { LV_STARTMETHOD, LV_START, LV_PLAY, LV_END };

    private static HideAndSeekManager m_instance = null;
    public static HideAndSeekManager Instance => m_instance;

    [SerializeField] private TMP_Text m_timerTxt;
    [SerializeField] private GameObject m_gametxt;
    [SerializeField] private Slider m_barSlider;
    [SerializeField] private HideAndSeekHeart m_Heart;
    [SerializeField] private GameObject m_darkPanel;
    [SerializeField] private GameObject m_retryButton;
    [SerializeField] private GameObject m_homeButton;
    [SerializeField] private GameObject m_startPanel;
    [SerializeField] private GameObject m_methodPanel;

    private LEVEL m_level = LEVEL.LV_STARTMETHOD;
    private Image m_gametxtImage;
    private RectTransform m_gametxtTransform;

    private GameObject m_player;
    private bool m_gamePlay = false;
    private bool m_gameStop = false;
    private bool m_monitor  = false;
    private bool m_pause    = false;
    private bool m_gameFinish = false;
    private float m_timer = 60f;
    private float m_wait  = 0f;
    private float m_speed = 1f;

    private bool  m_reversal = false;
    private float m_reversalTime = 0f;

    private float m_itemTime = 0;
    private float m_itemCreate = 0;
    private float m_itemCreateMin = 5;
    private float m_itemCreateMax = 10;

    private float m_maxScale = 4f;
    private Vector3 m_initialScale;
    private int m_beforItemIndex = -1;

    private GameObject m_goalFlag = null;

    public LEVEL CurrentLevel
    {
        get => m_level;
        set => m_level = value;
    }

    public bool GamePlay => m_gamePlay;
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
    public bool Pause
    {
        get => m_pause;
        set => m_pause = value;
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
        {
            m_instance = this;
            m_player = GameObject.FindGameObjectWithTag("Player");

            m_itemCreate   = Random.Range(m_itemCreateMin, m_itemCreateMax);
            m_gametxtImage     = m_gametxt.GetComponent<Image>();
            m_gametxtTransform = m_gametxt.GetComponent<RectTransform>();
            m_initialScale     = m_gametxtTransform.localScale;
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if(m_level == LEVEL.LV_STARTMETHOD)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    m_methodPanel.SetActive(false);
                    m_startPanel.SetActive(true);
                    m_level = LEVEL.LV_START;
                }
            }
        }
        else if(m_level == LEVEL.LV_PLAY)
        {
            if (m_pause == true) // 일시정지
                return;

            if (m_gamePlay)
            {
                m_timer -= Time.deltaTime;
                if (m_timer <= 0.8f)
                {
                    m_timerTxt.text = "00:00";
                    Finish_Game("FAIL");
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
            else if (m_gameFinish)
            {
                //m_gametxtTransform.localScale += Vector3.one * 0.08f;
                //if (m_gametxtTransform.localScale.x > m_maxScale)
                //{
                //    m_gametxtTransform.localScale = Vector3.one * m_maxScale;

                //    m_wait += Time.deltaTime;
                //    if (m_wait > 0.2f)
                //    {
                //        m_wait = 0f;

                //        m_retryButton.SetActive(true);
                //        m_homeButton.SetActive(true);
                //    }
                //}
            }
        }
    }

    public void Start_Game()
    {
        m_gamePlay = true;
        m_level = LEVEL.LV_PLAY;
        m_player.GetComponent<Animator>().StopPlayback(); // 애니메이션 재생
    }

    public void Scroll_Game(float touchDir)
    {
        if (m_gamePlay == false || m_monitor == true || m_pause == true) // 게임 시작 여부/ 감시자 효과 / 일시정지
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
            Finish_Game("CLEAR");
            Clear_Game();
        }
        else
        {
            if(touchDir == 1f) // 전진시에만 생성
                Crate_Item();
        }
    }

    private void Finish_Game(string str)
    {
        m_gamePlay = false;
        m_darkPanel.SetActive(true);
        m_gametxt.SetActive(true);
        m_player.GetComponent<Animator>().StartPlayback();

        m_gameFinish = true;

        if (str == "CLEAR")
            m_gametxtImage.sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Clear");
        else if(str == "FAIL")
            m_gametxtImage.sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Fail");
        m_gametxtTransform.localScale = m_initialScale;// * 0.1f;
    }

    private void Clear_Game()
    {
        // 남은 타이머에 따라 별 개수 차등 분배
    }

    public void Update_Heart(int heartCount)
    {
        m_Heart.Update_Heart(heartCount);
        if (heartCount == 0)
        {
            // 게임 실패
            Finish_Game("FAIL");
        }
    }

    public void Button_Retry()
    {
        SceneManager.LoadScene("HideAndSeek"); // 해당 씬 재시작
    }

    public void Button_Home()
    {
        // SceneManager.LoadScene("Home");
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
        m_pause = true;
        m_methodPanel.SetActive(true);
    }


    public void False_Pause()
    {
        StartCoroutine(Wait_Pause());
    }

    private IEnumerator Wait_Pause()
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_pause = false;
        yield break;
    }
}
