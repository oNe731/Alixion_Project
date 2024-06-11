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
        m_dialogs[0] = "�̰���" + "\n" + "�¾�� 3��° �༺�� ����," + "\n" + "���� ���� �� �ϳ��̸� �ô�� �߼�.";
        m_dialogs[1] = "�̰����� ���ο� ������ ź���� �������� ����" + "\n" + "���Ӱ� �پ��� \"���ɼ�\" �� �߾��Ϸ��� �Ѵ�....";
        m_dialogs[2] = "�� �ΰ��� ���ѵ鿡�� ���ݴ���" + "\n" + "����� ���⸦ ������ �ִ�." + "\n" + "�ΰ��� ���� �ױ� �������� �ٴٸ���" + "\n" + "������ ������ �ϰ��� �������� ����!";
        m_dialogs[3] = "�ϴÿ��� ���� ��½�̴���," + "\n" + "������ �ĵκο� ��Ÿ!";
        m_dialogs[4] = "���� ������ ��������" + "\n" + "�ΰ��� �ڽ��� ������" + "\n" + "�ǹ��� Ÿ���� ��ü�� �ٶ󺻴�.";
        m_dialogs[5] = "��ü�� ��Ʋ�Ÿ�����" + "\n" + "���İ��� ����ü�� ź���ϴ� ������ ���δ�.";
        m_dialogs[6] = "������ ��� ������ �Ϳ��ٰ� �ϴ���";
        m_dialogs[7] = "�ΰ���" + "\n" + "�ڽŰ� �ٸ� �������� �Ѵ��� �˾ƺ�������" + "\n" + "���̷Ӱ� �������ְ� ���� �ڽ��� ������ ���� �� ������ ��½ ���ø���.";
        m_dialogs[8] = "������ �谡 ������ ����ؼ�" + "\n" + "�� ���� �ƴ� �ϳ��ۿ� ���� ������" + "\n" + "�ڽ��� ������ ��Ƴ���" + "\n" + "�Կ����� �����Ҹ��� ���հ� �ִ�.";
        m_dialogs[9] = "�ΰ��� ���� ��Ȳ������," + "\n" + "����ü���� �ڽ��� ����� ������ �밡�μ�" + "\n" + "������ Ű���ֱ�� �Ѵ�.";
        m_dialogs[10] = "������ ���� ���ƿ����� �ΰ��� ������" + "\n" + "��� Ű���� ���� ��������" + "\n" + "������ ǥ���� �Ѵ�.";
        m_dialogs[11] = "������� �Կ��� ǥ���Ϸ��� ����," + "\n" + "������ ���ڱ� ���̸� ����!";
        m_dialogs[12] = "������ ���̸� �հ��� ���Է� ���÷�" + "\n" + "���̸� ���� �ΰ���" + "\n" + "����� ����� ǥ���� �µ��������� �������" + "\n" + "�ڽ��� �ؾ� �� ���� ���ø���.";

        m_letters = new string[12];
        m_letters[0] = "<�� �Ʊ⸦ Ű���ּ���>";
        m_letters[1] = "�� �Ʊ�� ���� �༺�� ���ο� ����ü�ν�" + "\n" + "�پ��� ���ɼ��� ǰ���ֽ��ϴ�.";
        m_letters[2] = "�� ���� �а� �ִٸ�" + "\n" + "����� �� ������ �θ��� ���Դϴ�." + "\n" + "����, ����� �и� ���̸� Ű����" + "\n" + "å���� ���� ���Դϴ�.";
        m_letters[3] = "�׷��� ������ �������� ���ð�" + "\n" + "�Ʊ��� ���ɼ��� ������ ���Ѻ��ּ���.";
        m_letters[4] = "�Ʊ�� �ΰ��� 5���� Ư���� Ư����" + "\n" + "�԰� ��ư��ϴ�.";
        m_letters[5] = "\"��, �ĸ�, �⸸, ����, ����\" �� �ټ�������" + "\n" + "���� �����̻� ���̰� �ȴٸ�," + "\n" + "�и� ����� ������" + "\n" + "���ο� ���ɼ����� ź���� ���Դϴ�.";
        m_letters[6] = "�Ʊ�� �ִ� 2������ Ư����" + "\n" + "������ ���� �� �ֽ��ϴ�.";
        m_letters[7] = "��� �Ǹ� ���̶� ���� Ư���� �ݿ��Ͽ�" + "\n" + "������ �ڽ��� ���ɼ��� ������ ���Դϴ�.";
        m_letters[8] = "����� �Ʊ�� �Բ��Ͽ�" + "\n" + "�������� ������ �ΰ��� Ư���� ����ȭ�� ����" + "\n" + "���� �� ���� ���Դϴ�.";
        m_letters[9] = "�� Ư���� ������ �������� ���̰� �Ǹ�" + "\n" + "���̴� �и� ������ ���Դϴ�.";
        m_letters[10] = "�ε� ���̸� ������ Ű���" + "\n" + "��.��.��.�� ���ɼ��� ���Ѻ��ּ���";
        m_letters[11] = "������ ���ϴ�.";
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
