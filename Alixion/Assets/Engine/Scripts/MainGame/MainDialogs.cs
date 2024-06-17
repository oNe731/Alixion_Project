using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainDialogs : MonoBehaviour
{
    [SerializeField] private GameObject m_introPanel;
    [SerializeField] private GameObject m_namePanel;
    [SerializeField] private TMP_Text   m_text;

    private string[] m_dialogs;
    private int m_currentDialogIndex = 0;
    private float m_typingSpeed = 0.05f;
    private Coroutine m_typingCoroutine = null;

    private void Awake()
    {
        m_dialogs = new string[12];
        m_dialogs[0] = "이곳은 중세 시대 한 유럽 국가의 으슥한 골목.";
        m_dialogs[1] = "괴한에게 쫓기고 있는 한 인간은" + "\n" + "절명할 순간 하늘에서 떨어진" + "\n" + "생명체에게 구원을 받는다.";
        m_dialogs[2] = "생명체는 특수한 아기로," + "\n" + "이 아이를 잘 키워달라는" + "\n" + "메세지도 같이 포함되어 있다.";
        m_dialogs[3] = "인간은 생명의 은인에게" + "\n" + "해야 할 책무를 다하기로 한다.";

        m_dialogs[4] = "외계인 생명체는" + "\n" + "5가지의 특수한 아이템의 특성을" + "\n" + "먹고 성장합니다.";
        m_dialogs[5] = "생명체는 최대 최상위의" + "\n" + "2가지의 특성을 가지고 있을 수 있습니다." + "\n" + "다른 특성이 5개 있더라도" + "\n" + "가장 높은 포인트를 가진 특성 2개가 선택됩니다.";
        m_dialogs[6] = "5가지의 아이템은" + "\n" + "5가지의 미니게임에 따라 각각 얻을 수 있습니다." + "\n" + "미니게임은 각각" + "\n" + "[선], [파멸], [사기], [은둔], [광기]의" + "\n" + "테마를 가지고 있습니다.";
        m_dialogs[7] = "미니게임이 어떤 테마인지 알려주진 않지만" + "\n" + "게임을 통해 어떤 테마일지 유추할 수 있습니다.";
        m_dialogs[8] = "미니게임을 성공적으로 클리어했다면" + "\n" + "각 미니게임의 기준에 따라" + "\n" + "더 많이, 더 빠르게" + "\n" + "좋은 보상이 주어집니다.";
        m_dialogs[9] = "각 테마의 좋은 보상은" + "\n" + "아기의 특정 테마의 특성 포인트를" + "\n" + "보다 빠르게 성장시킬 수 있습니다.";
        m_dialogs[10] = "플레이어는 미니게임을 통해 아이템을 습득하고" + "\n" + "생명체에게 어떤 먹이를 줄지" + "\n" + "고민하고 선택하여 먹이고를 반복하면," + "\n" + "포인트의 총 합이 일정 수준에 도달하게 되는데," + "\n" + "도달 시 엔딩을 볼 수 있습니다.";
        m_dialogs[11] = "플레이어 여러분은 특수한 조합에 따라" + "\n" + "15가지의 숨겨진 엔드 카드를 볼 수 있습니다.";
    }

    private void Update()
    {
        if (m_introPanel == null)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Input.GetTouch(0).position.y > (Screen.height * 5 / 6))
                return;

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
                    //Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/MainBGM");
                    //Camera.main.GetComponent<AudioSource>().Play();

                    //Destroy(m_introPanel);
                    Destroy(m_introPanel);
                    m_namePanel.SetActive(true);
                }
            }
        }
    }

    public void Start_Dialogs()
    {
        if (m_introPanel == null)
            return;

        if (m_dialogs.Length > 0)
        {
            m_introPanel.SetActive(true);
            StartTyping(m_dialogs[m_currentDialogIndex]);
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

    public void Skip_Button()
    {
        //Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/MainBGM");
        //Camera.main.GetComponent<AudioSource>().Play();

        Destroy(m_introPanel);
        m_namePanel.SetActive(true);
    }
}
