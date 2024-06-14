using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private LevelManager m_levelManager;

    private Image         m_txtImage;
    private RectTransform m_txtTransform;

    private bool  m_start = false;
    private float m_wait  = 0f;

    private float   m_maxScale = 4f;
    private Vector3 m_initialScale;

    private void Awake()
    {
        m_txtImage     = transform.GetChild(0).GetComponent<Image>();
        m_txtTransform = m_txtImage.GetComponent<RectTransform>();
        m_initialScale = m_txtTransform.localScale;
    }

    private void Update()
    {
        //if (!m_start && Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        m_start = true;
        //        transform.GetChild(0).gameObject.SetActive(true);
        //        m_txtImage.sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Ready");
        //        m_txtTransform.localScale = m_initialScale * 0.1f;
        //    }
        //}
        //else
        if (m_start)
        {
            m_txtTransform.localScale += Vector3.one * 0.07f;
            if (m_txtTransform.localScale.x > m_maxScale)
            {
                m_txtTransform.localScale = Vector3.one * m_maxScale;
                if (m_txtImage.sprite.name != "Start")
                {
                    m_wait += Time.deltaTime;
                    if (m_wait > 0.2f)
                    {
                        m_wait = 0f;
                        m_txtImage.sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Start");
                        m_txtTransform.localScale = m_initialScale * 0.1f;
                    }
                }
                else
                {
                    m_wait += Time.deltaTime;
                    if (m_wait > 0.2f)
                    {
                        m_wait = 0f;

                        m_levelManager.Start_Game();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void Start_Ready()
    {
        m_start = true;
        transform.GetChild(0).gameObject.SetActive(true);
        m_txtImage.sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Ready");
        m_txtTransform.localScale = m_initialScale * 0.1f;
    }
}
