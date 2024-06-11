using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekSign : MonoBehaviour
{
    private bool m_pause = false;
    private float m_time = 0f;

    private float m_pausTime = 0f;
    private float m_pausTimeMin = 3f;
    private float m_pausTimeMax = 6f;

    private GameObject m_timer = null;

    public float TimeInfo
    {
        get { return (m_time - 0f) / (m_pausTime - 0f); }
    }

    private void Update()
    {
        if (GameManager.Instance.Pause == true)
            return;

        if (m_pause)
        {
            m_time += Time.deltaTime;
            if (m_time > m_pausTime)
            {
                m_time = 0f;
                m_pause = false;

                HideAndSeekManager.Instance.GameStop = false;

                //GameObject player = GameObject.FindGameObjectWithTag("Player");
                //player.GetComponent<SpriteRenderer>().color = Color.white;

                if (m_timer != null)
                    Destroy(m_timer);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 전진 불가능
        HideAndSeekManager.Instance.GameStop = true;

        // UI 아이콘 생성
        if(m_timer == null)
        {
            m_timer = Instantiate(Resources.Load<GameObject>("Prefabs/MiniGame/Seclusion_Game/UI/Timer"), GameObject.Find("Canvas").transform.GetChild(0).transform);
            m_timer.GetComponent<HideAndSeekTimer>().Owner = gameObject;
        }

        m_pause = true;
        m_pausTime = Random.Range(m_pausTimeMin, m_pausTimeMax);

        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 후진 가능
        HideAndSeekManager.Instance.GameStop = false;
    }
}
