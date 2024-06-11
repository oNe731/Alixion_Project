using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenButton : MonoBehaviour
{
    public void Play_Game()
    {
        GetComponent<AudioSource>().Play();
        if (GameManager.Instance.Tutorial == false)
        {
            Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => SceneManager.LoadScene("Intro"), 0f, false);
        }
        else
        {
            Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => SceneManager.LoadScene("MainGame"), 0f, false);
        }
    }

    public void Open_Settings()
    {
        GetComponent<AudioSource>().Play();
        GameManager.Instance.Open_Settings();
    }

    public void Exit_Game()
    {
        GetComponent<AudioSource>().Play();
        GameManager.Instance.Exit_Game();
    }

    public void Open_Encyclopedia()
    {
        GetComponent<AudioSource>().Play();
        GameManager.Instance.Open_Encyclopedia();
    }
}
