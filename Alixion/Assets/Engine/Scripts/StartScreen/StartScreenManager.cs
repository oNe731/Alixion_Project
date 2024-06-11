using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.gameObject.GetComponent<Setting>().Update_AllAudioSources();
        Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

        GameObject button = null;
        if(GameManager.Instance.Encyclopedia.Get_EmptyEncyclopedia() == true)
        {
            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Play"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 7f);

            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Setting"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -237f);

            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Exit"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -467f);
        }
        else
        {
            // ������ 1�� �̻� ������ �� ���� ��ư ����
            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Play"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 7f);

            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Encyclopedia"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -237f);

            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Setting"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -467f);

            button = Instantiate(Resources.Load<GameObject>("Prefabs/StartScreen/Button_Exit"), GameObject.Find("Canvas").transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -697f);
        }
    }
}