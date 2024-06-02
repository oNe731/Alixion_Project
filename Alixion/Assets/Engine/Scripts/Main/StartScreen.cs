using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("����ȭ������");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Debug.Log("���� ����");
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Debug.Log("���� ����");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("����");
    }
}


