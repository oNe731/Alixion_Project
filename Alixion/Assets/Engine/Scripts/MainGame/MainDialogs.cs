using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainDialogs : MonoBehaviour
{
    [SerializeField] private GameObject m_Panel;
    [SerializeField] private TMP_Text m_text;

    private string[] m_dialogs;
    private int m_currentDialogIndex = 0;
    private float m_typingSpeed = 0.05f;
    private Coroutine m_typingCoroutine = null;

    private void Awake()
    {
        m_dialogs = new string[12];
        m_dialogs[0] = "이곳은 중세시대 한 유럽국가의 으슥한 골목.";
        m_dialogs[1] = "괴한에게 쫓기고 있는 한 인간은" + "\n" + "절명할 순간 하늘에서 떨어진" + "\n" + "생명체에게 구원을 받는다.";
        m_dialogs[2] = "생명체는 특수한 아기로," + "\n" + "이 아이를 잘 키워달라는" + "\n" + "메세지도 같이 포함되어 있다.";
        m_dialogs[3] = "인간은 생명의 은인에게" + "\n" + "해야할 책무를 다하기로 한다.";

        m_dialogs[4] = "외계인 생명체는" + "\n" + "5가지의 특수한 아이템의 특성을" + "\n" + "먹고 성장합니다.";
        m_dialogs[5] = "생명체는 최대 최상위의" + "\n" + "2가지의 특성을 가지고 있을 수 있습니다." + "\n" + "다른 특성이 5개 있더라도" + "\n" + "가장 포인트를 가진 특성 2개가 선택됩니다.";
        m_dialogs[6] = "5가지의 아이템은" + "\n" + "5가지의 미니게임에 따라 얻을 수 있습니다." + "\n" + "미니게임은 각각" + "\n" + "[선], [파멸], [사기], [은둔], [광기]의" + "\n" + "테마를 가지고 있습니다.";
        m_dialogs[7] = "미니게임이 어떤 테마인지 글로 알려주진 않지만" + "\n" + "게임을 통해 어떤 테마일지 유추할 수 있습니다.";
        m_dialogs[8] = "미니게임을 성공적으로 클리어 했다면" + "\n" + "각 미니게임의 기준에 따라" + "\n" + "더 빠르게, 더 많이," + "\n" + "더 성공적으로 클리어 할 수록" + "\n" + "좋은 보상이 주어집니다.";
        m_dialogs[9] = "각 테마의 좋은 보상은" + "\n" + "아기의 특정 테마의 특성포인트를" + "\n" + "보다 빠르게 성장시킬 수 있습니다.";
        m_dialogs[10] = "플레이어는 미니게임을 통해 아이템을 습득하고" + "\n" + "생명체에게 어떤 먹이를 줄지" + "\n" + "고민하고 선택하며 먹이고를 반복하여" + "\n" + "먹인 아이템의 특성 5가지를 포함한" + "\n" + "포인트의 총 합이 일정 수준에 도달하면" + "\n" + "엔딩을 볼 수 있습니다.";
        m_dialogs[11] = "특수한 조합에 따라 플레이어 여러분은" + "\n" + "15가지의 숨겨진 엔드카드를 볼 수 있습니다.";
    }

    private void Update()
    {
        if (m_Panel == null)
            return;

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
                    Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/MainBGM");
                    Camera.main.GetComponent<AudioSource>().Play();

                    Destroy(m_Panel);
                }
            }
        }
    }

    public void Start_Dialogs()
    {
        if (m_Panel == null)
            return;

        if (m_dialogs.Length > 0)
        {
            m_Panel.SetActive(true);
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
}
