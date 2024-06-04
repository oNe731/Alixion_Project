using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    #region BUTTON
    public void Play_Game()
    {
        UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => SceneManager.LoadScene("MainGame"), 0f, false);
    }

    public void Open_Settings()
    {
        GameManager.Instance.Open_Settings();
    }

    public void Exit_Game()
    {
        GameManager.Instance.Exit_Game();
    }
    #endregion
}