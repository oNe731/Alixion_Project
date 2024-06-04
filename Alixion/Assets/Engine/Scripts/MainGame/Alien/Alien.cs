using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_BASIC;
    private Animator m_animator = null;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_currentAlienType = GameManager.Instance.CurrentAlienType;
        m_animator.runtimeAnimatorController = GameManager.Instance.Get_AlionAnimator(0);
    }

    private void Update()
    {
        if(m_currentAlienType != GameManager.Instance.CurrentAlienType)
        {
            m_currentAlienType = GameManager.Instance.CurrentAlienType;
            m_animator.runtimeAnimatorController = GameManager.Instance.Get_AlionAnimator(0);
        }
    }
}
