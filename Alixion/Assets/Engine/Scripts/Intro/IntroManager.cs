using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private static IntroManager m_instance = null;
    public static IntroManager Instance => m_instance;

    private enum STATE { ST_INTRO, ST_LETTER_CREATE, ST_LETTER_OPEN, ST_END };

    [SerializeField] GameObject m_letter;
    [SerializeField] TMP_Text m_text;

    private STATE m_state = STATE.ST_INTRO;
    private string[] m_dialogs;
    private string[] m_letters;
    private int   m_currentDialogIndex = 0;
    private float m_typingSpeed = 0.05f;
    private Coroutine m_typingCoroutine = null;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        GameManager.Instance.gameObject.GetComponent<Setting>().Update_AllAudioSources();
        Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

        m_dialogs = new string[13];
        m_dialogs[0] = "이곳은" + "\n" + "태양계 3번째 행성의 지구," + "\n" + "유럽 국가 중 하나이며 시대는 중세.";
        m_dialogs[1] = "이곳에서 새로운 생명의 탄생과 성장으로 인해" + "\n" + "새롭고 다양한 \"가능성\" 이 발아하려고 한다....";
        m_dialogs[2] = "한 인간이 괴한들에게 습격당해" + "\n" + "목숨의 위기를 느끼고 있다." + "\n" + "인간은 정말 죽기 직전까지 다다르고" + "\n" + "괴한이 최후의 일격을 날리려는 순간!";
        m_dialogs[3] = "하늘에서 빛이 번쩍이더니," + "\n" + "괴한의 후두부에 강타!";
        m_dialogs[4] = "이후 괴한은 쓰러지고" + "\n" + "인간은 자신을 구원한" + "\n" + "의문의 타원형 물체를 바라본다.";
        m_dialogs[5] = "물체는 꿈틀거리더니" + "\n" + "순식간에 생명체가 탄생하는 순간을 보인다.";
        m_dialogs[6] = "세상의 모든 새끼는 귀엽다고 하던가";
        m_dialogs[7] = "인간은" + "\n" + "자신과 다른 종족임을 한눈에 알아보았지만" + "\n" + "경이롭고 깨물어주고 싶은 자신의 감각을 참고 그 새끼를 번쩍 들어올린다.";
        m_dialogs[8] = "새끼는 배가 고픈지 계속해서" + "\n" + "한 쌍이 아닌 하나밖에 없는 눈에서" + "\n" + "자신의 눈물을 쏟아내고" + "\n" + "입에서는 울음소리를 내뿜고 있다.";
        m_dialogs[9] = "인간은 순간 당황했지만," + "\n" + "생명체에게 자신의 목숨을 구해준 대가로서" + "\n" + "새끼를 키워주기로 한다.";
        m_dialogs[10] = "집으로 같이 돌아왔지만 인간은 새끼를" + "\n" + "어떻게 키워야 할지 막막함을" + "\n" + "몸으로 표현을 한다.";
        m_dialogs[11] = "답답함을 입에서 표현하려던 찰나," + "\n" + "새끼가 갑자기 종이를 뱉어낸다!";
        m_dialogs[12] = "찝찝한 종이를 손가락 집게로 들어올려" + "\n" + "종이를 읽은 인간은" + "\n" + "방금의 답답한 표정은 온데간데없이 밝아지고" + "\n" + "자신이 해야 할 일을 떠올린다.";

        m_letters = new string[12];
        m_letters[0] = "<이 아기를 키워주세요>";
        m_letters[1] = "이 아기는 저희 행성의 새로운 생명체로써" + "\n" + "다양한 가능성을 품고있습니다.";
        m_letters[2] = "이 글을 읽고 있다면" + "\n" + "당신이 이 아이의 부모일 것입니다." + "\n" + "또한, 당신은 분명 아이를 키워낼" + "\n" + "책임이 있을 것입니다.";
        m_letters[3] = "그러니 끝까지 포기하지 마시고" + "\n" + "아기의 가능성을 끝까지 지켜봐주세요.";
        m_letters[4] = "아기는 인간의 5가지 특수한 특성을" + "\n" + "먹고 살아갑니다.";
        m_letters[5] = "\"선, 파멸, 기만, 은둔, 광기\" 이 다섯가지를" + "\n" + "일정 수준이상 먹이게 된다면," + "\n" + "분명 당신이 선택한" + "\n" + "새로운 가능성으로 탄생할 것입니다.";
        m_letters[6] = "아기는 최대 2가지의 특성을" + "\n" + "가지고 있을 수 있습니다.";
        m_letters[7] = "어른이 되면 아이때 얻은 특성을 반영하여" + "\n" + "스스로 자신의 가능성을 보여줄 것입니다.";
        m_letters[8] = "당신은 아기와 함께하여" + "\n" + "여러가지 행위로 인간의 특성을 물질화한 것을" + "\n" + "얻을 수 있을 것입니다.";
        m_letters[9] = "이 특성을 내포한 물질들을 먹이게 되면" + "\n" + "아이는 분명 성장할 것입니다.";
        m_letters[10] = "부디 아이를 끝까지 키우고" + "\n" + "여.러.가.지 가능성을 지켜봐주세요";
        m_letters[11] = "무운을 빕니다.";
    }

    private void Start()
    {
        if (m_dialogs.Length > 0)
        {
            StartTyping(m_dialogs[m_currentDialogIndex]);
        }
    }

    private void Update()
    {
        switch(m_state)
        {
            case STATE.ST_INTRO:
                Update_Intro();
                break;

            case STATE.ST_LETTER_OPEN:
                Update_Letter();
                break;
        }
    }

    private void Update_Intro()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (m_typingCoroutine != null)
            {
                StopCoroutine(m_typingCoroutine);
                m_typingCoroutine = null;
                m_text.text = m_dialogs[m_currentDialogIndex];
            }
            else
            {
                m_currentDialogIndex++;
                if (m_currentDialogIndex < m_dialogs.Length)
                {
                    StartTyping(m_dialogs[m_currentDialogIndex]);
                }
                else
                {
                    m_state = STATE.ST_LETTER_CREATE;
                    m_text.text = "";
                    Instantiate(Resources.Load<GameObject>("Prefabs/Intro/Button_Letter"), GameObject.Find("Canvas").transform);
                }
            }
        }
    }

    private void Update_Letter()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (m_typingCoroutine != null)
            {
                StopCoroutine(m_typingCoroutine);
                m_typingCoroutine = null;
                m_text.text = m_letters[m_currentDialogIndex];
            }
            else
            {
                m_currentDialogIndex++;
                if (m_currentDialogIndex < m_letters.Length)
                {
                    StartTyping(m_letters[m_currentDialogIndex]);
                }
                else
                {
                    Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => SceneManager.LoadScene("MainGame"), 0f, false);
                }
            }
        }
    }

    private void StartTyping(string dialog)
    {
        m_text.text = "";
        m_typingCoroutine = StartCoroutine(TypeText(dialog));
    }

    private IEnumerator TypeText(string dialog)
    {
        foreach (char letter in dialog.ToCharArray())
        {
            m_text.text += letter;
            yield return new WaitForSeconds(m_typingSpeed);
        }
        m_typingCoroutine = null;
    }

    public void Start_Letter()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();

        m_state = STATE.ST_LETTER_OPEN;

        m_letter.SetActive(true);
        m_text.color = Color.black;

        m_currentDialogIndex = 0;
        m_typingCoroutine = null;

        if (m_letters.Length > 0)
        {
            StartTyping(m_letters[m_currentDialogIndex]);
        }
    }
}
