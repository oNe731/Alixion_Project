using UnityEngine;

public class Button : MonoBehaviour
{
    // ��ư ���� ��ȣ (�ν����� â���� ���� ����)
    public int buttonNumber;

    // ��ư Ŭ�� �̺�Ʈ ó�� (��: HandleButtonInput �Լ� ȣ��)
    void OnClick()
    {
        HandleButtonInput();
    }

    void HandleButtonInput()
    {
        // Ư�� ��Ȳ �߻� (��: ��ư �� ����, ���� ���, ���ο� �� �ε�)
        // ...
    }
}
