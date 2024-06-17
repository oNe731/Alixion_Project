using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject m_panelMiniGame;

    [SerializeField] private GameObject m_tutorialPanel;
    [SerializeField] private GameObject m_nameInputPanel;

    private void Start()
    {
        GameManager.Instance.gameObject.GetComponent<Setting>().Update_AllAudioSources();
        Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

        if (GameManager.Instance.Tutorial == false)
        {
            Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/IntroBGM");
            Camera.main.GetComponent<AudioSource>().Play();

            // 튜토리얼 재생
            GameManager.Instance.Tutorial = true;
            GetComponent<MainDialogs>().Start_Dialogs();

            GameManager.Instance.ActivePanel = true;
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/BGM/MainBGM");
            Camera.main.GetComponent<AudioSource>().Play();

            Destroy(m_tutorialPanel);
            Destroy(m_nameInputPanel);
        }
    }

    #region BUTTON
    public void Open_Profile()
    {
        GameManager.Instance.Open_Profile();
    }

    public void Close_Profile()
    {
        GameManager.Instance.Close_Profile();
    }

    public void Open_Inventory()
    {
        GameManager.Instance.Open_Inventory();
    }

    public void Close_Inventory()
    {
        GameManager.Instance.Close_Inventory();
    }

    public void Open_Encyclopedia()
    {
        GameManager.Instance.Open_Encyclopedia();
    }

    public void Close_Encyclopedia()
    {
        GameManager.Instance.Close_Encyclopedia();
    }

    public void Open_Settings()
    {
        GameManager.Instance.Open_Settings();
    }

    public void Close_Settings()
    {
        GameManager.Instance.Close_Settings();
    }

    public void Exit_Game()
    {
        GameManager.Instance.Exit_Game();
    }

    public void Button_Minigame()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();

        if (m_panelMiniGame.activeSelf == false)
            m_panelMiniGame.SetActive(true);
        else
            m_panelMiniGame.SetActive(false);
    }
    #endregion

    #region SCENE
    public void Load_RuinScene()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.Portrait, "Destory")), 0f, false); // 세로
    }

    public void Load_ZenScene()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeLeft, "Zen")), 0f, false); // 가로
    }

    public void Load_FraudScene()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.Portrait, "Cheat")), 0f, false); // 세로
    }

    public void Load_SeclusionScene()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeLeft, "HideAndSeek")), 0f, false); // 가로
    }

    public void Load_MadnessScene()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeLeft, "Madness")), 0f, false); // 가로
    }
    #endregion
}
