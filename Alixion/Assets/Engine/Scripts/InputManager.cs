using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public ButtonManager buttonManager; // ButtonManager ��ũ��Ʈ ����

    void Update()
    {
        // �Էµ� ���ڸ� Ȯ��
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    int inputNumber = GetNumberFromKey(key); // �Էµ� ���� ���

                    if (inputNumber > 0 && inputNumber <= buttonManager.maxButtons)
                    {
                        // �Էµ� ���ڿ� �ش��ϴ� ��ư ã��
                        GameObject button = buttonManager.buttons.Find(btn => btn.GetComponent<Button>().buttonNumber == inputNumber);

                        if (button != null)
                        {
                            // Ư�� ��Ȳ �߻�
                            HandleButtonInput(button);
                        }
                    }
                }
            }
        }
    }

    int GetNumberFromKey(KeyCode key)
    {
        // �Էµ� Ű�� �ش��ϴ� ���� ��ȯ
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
        // ��ư Ŭ�� �� Ư�� ��Ȳ �߻� (��: ��ư �� ����, ���� ���, ���ο� �� �ε�)
        // ...

        // ����: ��ư �� ����
        button.GetComponent<SpriteRenderer>().color = Color.red;

        // ����: ���� ���
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // ����: ���ο� �� �ε�
        SceneManager.LoadScene("NewScene");
    }
}
