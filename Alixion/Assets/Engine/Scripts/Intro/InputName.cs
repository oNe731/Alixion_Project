using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : MonoBehaviour
{
    [Header("Namebox")]
    [SerializeField] private TMP_InputField m_playerNameInput;

    [Header("Popupbox")]
    [SerializeField] private GameObject m_popup;
    [SerializeField] private TMP_Text m_guide;

    private string[] m_randomnames;

    private void Awake()
    {
        // ��õ �г���
        m_randomnames = new string[10];
        m_randomnames[0] = "����";
        m_randomnames[1] = "��";
        m_randomnames[2] = "ȣ����";
        m_randomnames[3] = "��";
        m_randomnames[4] = "�䳢";
        m_randomnames[5] = "����";
        m_randomnames[6] = "�ϸ�";
        m_randomnames[7] = "��踻";
        m_randomnames[8] = "ǻ��";
        m_randomnames[9] = "��ī";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Select_Name();
    }

    public void Random_Name()
    {
        float randomNumber = Random.Range(0.0f, 9.9f);
        m_playerNameInput.text = m_randomnames[(int)randomNumber];
    }

    public void Select_Name()
    {
        if (m_playerNameInput.text.Length <= 0)
            return;

        m_guide.text = "\"" + m_playerNameInput.text + "\"" + "��\n�����Ͻðڽ��ϱ�?";
        m_popup.SetActive(true);
    }

    public void Popup_Yes()
    {
        GameManager.Instance.PlayerName = m_playerNameInput.text;

        Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/MainBGM");
        Camera.main.GetComponent<AudioSource>().Play();
        Destroy(gameObject);

        GameManager.Instance.ActivePanel = false;
    }

    public void Popup_No()
    {
        m_popup.SetActive(false);
    }
}
