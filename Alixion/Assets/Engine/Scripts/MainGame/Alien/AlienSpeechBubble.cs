using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlienSpeechBubble : MonoBehaviour
{
    [SerializeField] private GameObject m_speechBubble;
    [SerializeField] private TMP_Text m_text;

    private bool  m_active = false;
    private float m_activeTime = 0f;
    private int   m_brforeIndex = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(m_active == true)
        {
            if(GameManager.Instance.ActivePanel == true)
            {
                m_activeTime = 0f;

                m_active = false;
                m_speechBubble.SetActive(m_active);
                return;
            }

            m_activeTime += Time.deltaTime;
            if(m_activeTime > 2f)
            {
                m_activeTime = 0f;

                m_active = false;
                m_speechBubble.SetActive(m_active);
            }
        }
    }

    private void OnMouseDown()
    {
        if (m_active == true || GameManager.Instance.ActivePanel == true)
            return;

        AlienData alienData = GameManager.Instance.Get_AlienDate(GameManager.Instance.CurrentAlienType);

        m_active = true;
        m_speechBubble.SetActive(m_active);

        while(true)
        {
            int randomIndex = Random.Range(0, 3);
            if(m_brforeIndex != randomIndex)
            {
                m_brforeIndex = randomIndex;
                break;
            }
        }

        int level = GameManager.Instance.CurrentLevel - 1;
        if (level < 0)
            level = 0;
        m_text.text = alienData.Dialogs[level][m_brforeIndex];
    }
}
