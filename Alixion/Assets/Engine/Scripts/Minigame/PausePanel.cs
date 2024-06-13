using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public void Button_Resume()
    {
        gameObject.SetActive(false);
        GameManager.Instance.False_Pause();
    }

    public void Button_Retry()
    {
        gameObject.SetActive(false);
        GameManager.Instance.False_Pause();

        GameManager.Instance.Retry_Scene();
    }

    public void Button_MainScene()
    {
        gameObject.SetActive(false);
        GameManager.Instance.False_Pause();

        GameManager.Instance.Go_Home();
    }

    public void Button_Setting()
    {
        gameObject.SetActive(false);
        //GameManager.Instance.False_Pause();
        GameManager.Instance.Open_Settings();
    }
}
