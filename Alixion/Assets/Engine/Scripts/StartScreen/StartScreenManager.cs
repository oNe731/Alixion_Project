using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    #region BUTTON
    public void Play_Game()
    {
        SceneManager.LoadScene("MainGame");
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