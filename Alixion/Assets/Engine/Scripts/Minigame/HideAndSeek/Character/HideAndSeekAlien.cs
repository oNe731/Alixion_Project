using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HideAndSeekAlien : MonoBehaviour
{
    [SerializeField] private int m_hp = 5;
    [SerializeField] private Light2D m_light2D;
    private Animator m_animator;
    private Collider2D m_collider;
    private Coroutine m_coroutine = null;
    private bool m_dark = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
        m_animator.StartPlayback(); // 애니메이션 정지
    }

    private void Update()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        {
            if (touch.position.y > (Screen.height * 4 / 5))
                return;

            float dir = DetermineTouchSide(touch.position);
            if (HideAndSeekManager.Instance.Reversal == false)
                HideAndSeekManager.Instance.Scroll_Game(dir);
            else
                HideAndSeekManager.Instance.Scroll_Game(-dir);

            Check_Overlap(false);
        }
    }

    public void Check_Overlap(bool check = true)
    {
        bool hide = false;

        Bounds thisBounds = m_collider.bounds;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(thisBounds.center, thisBounds.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject) // 자신과 비교X
            {
                Bounds thisbounds = m_collider.bounds;
                Bounds otherbounds = collider.bounds;

                if (otherbounds.min.x <= thisbounds.min.x && thisbounds.max.x <= otherbounds.max.x)
                    hide = true;
            }
        }

        if(check == true && hide == false)
        {
            m_hp -= 1;
            HideAndSeekManager.Instance.Update_Heart(m_hp);
        }
        else if (check == false && hide == true)
        {
            // 어둠
            if (m_dark == true)
                return;

            m_dark = true;
            if (m_coroutine != null)
                StopCoroutine(m_coroutine);
            StartCoroutine(Fade_Light(m_light2D, new Color(0.12f, 0.12f, 0.12f, 1f), 0.1f));
        }
        else if (check == false && hide == false)
        {
            // 밝음
            if (m_dark == false)
                return;

            m_dark = false;
            if (m_coroutine != null)
                StopCoroutine(m_coroutine);
            StartCoroutine(Fade_Light(m_light2D, Color.white, 0.1f));
        }
    }

    private float DetermineTouchSide(Vector2 touchPosition)
    {
        if (touchPosition.x < Screen.width / 2)
            return -1f;
        else
            return 1f;
    }

    private IEnumerator Fade_Light(Light2D light, Color targetColor, float duration)
    {
        Color startColor = light.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            light.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }

        light.color = targetColor;
    }
}
