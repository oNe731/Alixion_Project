using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public ButtonManager buttonManager; // ButtonManager 스크립트 참조

    void Update()
    {
        // 입력된 숫자를 확인
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    int inputNumber = GetNumberFromKey(key); // 입력된 숫자 얻기

                    if (inputNumber > 0 && inputNumber <= buttonManager.maxButtons)
                    {
                        // 입력된 숫자에 해당하는 버튼 찾기
                        GameObject button = buttonManager.buttons.Find(btn => btn.GetComponent<Button>().buttonNumber == inputNumber);

                        if (button != null)
                        {
                            // 특정 상황 발생
                            HandleButtonInput(button);
                        }
                    }
                }
            }
        }
    }

    int GetNumberFromKey(KeyCode key)
    {
        // 입력된 키에 해당하는 숫자 반환
        if (key >= KeyCode.Alpha1 && key <= KeyCode.Alpha9)
        {
            return key - KeyCode.Alpha1 + 1;
        }
        else if (key >= KeyCode.Keypad1 && key <= KeyCode.Keypad9)
        {
            return key - KeyCode.Keypad1 + 1;
        }
        else
        {
            return 0;
        }
    }

    void HandleButtonInput(GameObject button)
    {
        // 버튼 클릭 시 특정 상황 발생 (예: 버튼 색 변경, 사운드 재생, 새로운 씬 로드)
        // ...

        // 예시: 버튼 색 변경
        button.GetComponent<SpriteRenderer>().color = Color.red;

        // 예시: 사운드 재생
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 예시: 새로운 씬 로드
        SceneManager.LoadScene("NewScene");
    }
}
