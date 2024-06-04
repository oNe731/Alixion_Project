using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotSelect : MonoBehaviour
{
    private Image m_selctImage;
    private float m_interval = 0.5f;
    private Coroutine m_coroutine = null;

    private void Start()
    {
        m_selctImage = GetComponent<Image>();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void Start_Coroutine()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(ChangeAlphaCoroutine());
    }

    private IEnumerator ChangeAlphaCoroutine()
    {
        while (true)
        {
            Set_Alpha(1f);
            yield return new WaitForSeconds(m_interval);

            Set_Alpha(0f);
            yield return new WaitForSeconds(m_interval);
        }
    }

    private void Set_Alpha(float alpha)
    {
        if (m_selctImage != null)
        {
            Color color = m_selctImage.color;
            color.a = alpha;

            m_selctImage.color = color;
        }
    }
}
