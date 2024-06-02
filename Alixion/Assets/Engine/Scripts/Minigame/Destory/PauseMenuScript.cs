using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    // ���� �Ͻ����� ��ư
    public GameObject pauseButton;

    // �Ͻ����� �޴� ĵ����
    public GameObject pauseMenuCanvas;

    // ���� ���� ��ư
    public GameObject resumeButton;

    // ���� ������ ��ư
    public GameObject quitButton;

    void Start()
    {
        // �Ͻ����� �޴� ĵ������ ��Ȱ��ȭ�մϴ�.
        pauseMenuCanvas.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        // Time.timeScale�� ����Ͽ� ���� �÷��� �ӵ��� 0���� �����Ͽ� ������ �Ͻ������մϴ�.
        Time.timeScale = 0;

        // RenderSettings.fogDensity�� ����Ͽ� ȭ�� ��⸦ 50% ��Ӱ� ����ϴ�.
        RenderSettings.fogDensity = 0.5f;

        // UI ĵ������ Ȱ��ȭ�Ͽ� "���� ������" �� "���� ����" ��ư�� ǥ���մϴ�.
        pauseMenuCanvas.SetActive(true);
    }

    public void OnResumeButtonClick()
    {
        // Time.timeScale�� ����Ͽ� ���� �÷��� �ӵ��� 1�� �����Ͽ� ������ �ٽ� �����մϴ�.
        Time.timeScale = 1;

        // RenderSettings.fogDensity�� ����Ͽ� ȭ�� ��⸦ ������� �ǵ����ϴ�.
        RenderSettings.fogDensity = 0;

        // UI ĵ������ ��Ȱ��ȭ�Ͽ� �޴��� ����ϴ�.
        pauseMenuCanvas.SetActive(false);
    }

    public void OnQuitButtonClick()
    {
        // Application.Quit() �Լ��� ȣ���Ͽ� ������ �����մϴ�.
        Application.Quit();
    }
}
