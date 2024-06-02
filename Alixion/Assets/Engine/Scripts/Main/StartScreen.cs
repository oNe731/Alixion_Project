using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("°ÔÀÓÈ­¸éÀ¸·Î");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Debug.Log("¼¼ÆÃ ÄÑÁü");
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Debug.Log("¼¼ÆÃ ²¨Áü");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("²¨Áü");
    }
}


