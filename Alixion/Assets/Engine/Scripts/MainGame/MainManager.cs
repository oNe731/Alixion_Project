using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject m_panelMiniGame;

    private void Start()
    {
        UIManager.Instance.Start_FadeIn(0.5f, Color.black);
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
        if(m_panelMiniGame.activeSelf == false)
            m_panelMiniGame.SetActive(true);
        else
            m_panelMiniGame.SetActive(false);
    }
    #endregion

    #region SCENE
    public void Load_RuinScene()
    {
        UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeRight, "Destory")), 0f, false);
    }

    public void Load_ZenScene()
    {
        //UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeRight, "Destory")), 0f, false);
    }

    public void Load_FraudScene()
    {
        UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeRight, "Cheat")), 0f, false);
    }

    public void Load_SeclusionScene()
    {
        UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeRight, "HideAndSeek")), 0f, false);
    }

    public void Load_MadnessScene()
    {
        //UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.LandscapeRight, "Destory")), 0f, false);
    }
    #endregion
}
