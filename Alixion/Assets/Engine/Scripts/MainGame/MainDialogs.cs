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
        m_dialogs[0] = "�̰��� �߼� �ô� �� ���� ������ ������ ���.";
        m_dialogs[1] = "���ѿ��� �ѱ�� �ִ� �� �ΰ���" + "\n" + "������ ���� �ϴÿ��� ������" + "\n" + "����ü���� ������ �޴´�.";
        m_dialogs[2] = "����ü�� Ư���� �Ʊ��," + "\n" + "�� ���̸� �� Ű���޶��" + "\n" + "�޼����� ���� ���ԵǾ� �ִ�.";
        m_dialogs[3] = "�ΰ��� ������ ���ο���" + "\n" + "�ؾ� �� å���� ���ϱ�� �Ѵ�.";

        m_dialogs[4] = "�ܰ��� ����ü��" + "\n" + "5������ Ư���� �������� Ư����" + "\n" + "�԰� �����մϴ�.";
        m_dialogs[5] = "����ü�� �ִ� �ֻ�����" + "\n" + "2������ Ư���� ������ ���� �� �ֽ��ϴ�." + "\n" + "�ٸ� Ư���� 5�� �ִ���" + "\n" + "���� ���� ����Ʈ�� ���� Ư�� 2���� ���õ˴ϴ�.";
        m_dialogs[6] = "5������ ��������" + "\n" + "5������ �̴ϰ��ӿ� ���� ���� ���� �� �ֽ��ϴ�." + "\n" + "�̴ϰ����� ����" + "\n" + "[��], [�ĸ�], [���], [����], [����]��" + "\n" + "�׸��� ������ �ֽ��ϴ�.";
        m_dialogs[7] = "�̴ϰ����� � �׸����� �˷����� ������" + "\n" + "������ ���� � �׸����� ������ �� �ֽ��ϴ�.";
        m_dialogs[8] = "�̴ϰ����� ���������� Ŭ�����ߴٸ�" + "\n" + "�� �̴ϰ����� ���ؿ� ����" + "\n" + "�� ����, �� ������" + "\n" + "���� ������ �־����ϴ�.";
        m_dialogs[9] = "�� �׸��� ���� ������" + "\n" + "�Ʊ��� Ư�� �׸��� Ư�� ����Ʈ��" + "\n" + "���� ������ �����ų �� �ֽ��ϴ�.";
        m_dialogs[10] = "�÷��̾�� �̴ϰ����� ���� �������� �����ϰ�" + "\n" + "����ü���� � ���̸� ����" + "\n" + "����ϰ� �����Ͽ� ���̰� �ݺ��ϸ�," + "\n" + "����Ʈ�� �� ���� ���� ���ؿ� �����ϰ� �Ǵµ�," + "\n" + "���� �� ������ �� �� �ֽ��ϴ�.";
        m_dialogs[11] = "�÷��̾� �������� Ư���� ���տ� ����" + "\n" + "15������ ������ ���� ī�带 �� �� �ֽ��ϴ�.";
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
