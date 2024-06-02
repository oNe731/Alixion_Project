using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab; // ��ư ������

    public int maxButtons = 15; // �ִ� ��ư ���� ����

    public List<GameObject> buttons = new List<GameObject>(); // ������ ��ư ���

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        for (int i = 0; i < maxButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, GetRandomPosition(), Quaternion.identity);
            newButton.transform.SetParent(transform); // ��ư�� �����ϴ� ������Ʈ�� �ڽ����� ����

            // ��ư�� ���� ��ȣ �Ҵ� (�ν����� â���� ������ �� ���)
            newButton.GetComponent<Button>().buttonNumber = i + 1; // Button ��ũ��Ʈ�� buttonNumber �Ӽ� �߰�

            buttons.Add(newButton); // ������ ��ư ��Ͽ� �߰�

            // ��ư ��ġ ���� (��ġ�� �ʵ���)
           
        }
    }

    Vector2 GetRandomPosition()
    {
        // ȭ�� ���� ������ ���� ��ġ ���
        float randomX = Random.Range(-2.0f, 2.0f);
        float randomY = Random.Range(-4.0f, 4.0f);

        return new Vector2(randomX, randomY);
    }


}
