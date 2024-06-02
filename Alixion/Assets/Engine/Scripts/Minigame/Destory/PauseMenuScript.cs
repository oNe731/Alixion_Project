using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    // 게임 일시정지 버튼
    public GameObject pauseButton;

    // 일시정지 메뉴 캔버스
    public GameObject pauseMenuCanvas;

    // 게임 진행 버튼
    public GameObject resumeButton;

    // 게임 나가기 버튼
    public GameObject quitButton;

    void Start()
    {
        // 일시정지 메뉴 캔버스를 비활성화합니다.
        pauseMenuCanvas.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        // Time.timeScale을 사용하여 게임 플레이 속도를 0으로 설정하여 게임을 일시정지합니다.
        Time.timeScale = 0;

        // RenderSettings.fogDensity를 사용하여 화면 밝기를 50% 어둡게 만듭니다.
        RenderSettings.fogDensity = 0.5f;

        // UI 캔버스를 활성화하여 "게임 나가기" 및 "게임 진행" 버튼을 표시합니다.
        pauseMenuCanvas.SetActive(true);
    }

    public void OnResumeButtonClick()
    {
        // Time.timeScale을 사용하여 게임 플레이 속도를 1로 설정하여 게임을 다시 시작합니다.
        Time.timeScale = 1;

        // RenderSettings.fogDensity를 사용하여 화면 밝기를 원래대로 되돌립니다.
        RenderSettings.fogDensity = 0;

        // UI 캔버스를 비활성화하여 메뉴를 숨깁니다.
        pauseMenuCanvas.SetActive(false);
    }

    public void OnQuitButtonClick()
    {
        // Application.Quit() 함수를 호출하여 게임을 종료합니다.
        Application.Quit();
    }
}
