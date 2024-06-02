using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HideAndSeekObserver : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprite;
    private SpriteRenderer m_spriteRenderer;

    private GameObject       m_playerObj;
    private HideAndSeekAlien m_player;
    private float m_smoothness = 5f;

    private float m_time = 0f;
    private float m_examine = 0f;
    private float m_examineMin = 3f;
    private float m_examineMax = 9f;
    private float m_waitTime = 1.5f;

    private bool m_check = false;

    private float m_shakeDuration  = 1.5f; // 흔들리는 시간
    private float m_shakeMagnitude = 0.1f; // 흔들리는 강도
    private Coroutine m_coroutine = null;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_playerObj = GameObject.FindGameObjectWithTag("Player");
        m_player    = m_playerObj.GetComponent<HideAndSeekAlien>();
        m_examine   = Random.Range(m_examineMin, m_examineMax);
    }

    private void LateUpdate()
    {
        if (HideAndSeekManager.Instance.GamePlay == false || HideAndSeekManager.Instance.Pause == true)
            return;

        m_time += Time.deltaTime;
        if(m_time >= m_examine)   // 조사하는 시간 주기
        {
            if (m_check == false) // 플레이어 은패 여부 검사 전조증상
            {
                if (m_coroutine != null)
                    StopCoroutine(m_coroutine);
                m_coroutine = StartCoroutine(Shake(() => m_coroutine = StartCoroutine(Check())));
            }
        }
        else // 따라가기
        {
            Vector3 targetPosition = m_playerObj.transform.position;
            targetPosition.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, m_smoothness * Time.deltaTime);
        }
    }

    private IEnumerator Shake(Action onComplete = null)
    {
        m_check = true;
        Vector3 m_originalPosition = transform.position;

        float time = 0.0f;
        while (time < m_shakeDuration)
        {
            if(HideAndSeekManager.Instance.Pause == true)
                yield return null;

            time += Time.deltaTime;

            float x = m_originalPosition.x + Random.Range(-m_shakeMagnitude, m_shakeMagnitude);
            transform.localPosition = new Vector3(x, m_originalPosition.y, m_originalPosition.z);
            yield return null;
        }

        transform.localPosition = m_originalPosition;

        if (HideAndSeekManager.Instance.Pause == true)
            yield return null;

        if (onComplete != null)
            onComplete?.Invoke();

        yield break;
    }

    private IEnumerator Check()
    {
        HideAndSeekManager.Instance.Monitor = true;
        m_spriteRenderer.sprite = m_sprite[1];
        m_player.Check_Overlap();

        float time = 0.0f;
        while (time < m_waitTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        HideAndSeekManager.Instance.Monitor = false;
        m_spriteRenderer.sprite = m_sprite[0];
        m_time    = 0f;
        m_check   = false;
        m_examine = Random.Range(m_examineMin, m_examineMax);

        yield break;
    }
}
