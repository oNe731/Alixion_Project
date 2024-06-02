using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAndSeekHeart : MonoBehaviour
{
    [SerializeField] private Image[]  m_heart;
    [SerializeField] private Sprite[] m_heartImage;

    public void Update_Heart(int heartCount)
    {
        for (int i = 0; i < heartCount; i++)
        {
            if (i >= m_heart.Length)
                return;

            m_heart[i].sprite = m_heartImage[0];
        }

        for (int i = heartCount; i < m_heart.Length; i++)
            m_heart[i].sprite = m_heartImage[1];
    }
}
