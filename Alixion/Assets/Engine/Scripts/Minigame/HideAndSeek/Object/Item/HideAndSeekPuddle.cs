using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekPuddle : MonoBehaviour
{
    private float m_beforSpeed = 0f;

    private float m_speedMin = 0.4f;
    private float m_speedMax = 0.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹 �� �ӵ� ���ο�
        if (other.gameObject.name == "Alien")
        {
            m_beforSpeed = HideAndSeekManager.Instance.Speed;
            HideAndSeekManager.Instance.Speed *= Random.Range(m_speedMin, m_speedMax);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // �÷��̾�� �浹 �� �ӵ� ����
        if (other.gameObject.name == "Alien")
        {
            HideAndSeekManager.Instance.Speed = m_beforSpeed;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
