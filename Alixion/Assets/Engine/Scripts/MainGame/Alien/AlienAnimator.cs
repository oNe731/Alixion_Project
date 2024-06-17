using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimator : MonoBehaviour
{
    [SerializeField] IMAGETYPE m_imageType = IMAGETYPE.IT_SPRITE;
    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_BASIC;
    private int m_currentLevel = 0;
    private Animator m_animator = null;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_currentAlienType = GameManager.Instance.CurrentAlienType;
        m_currentLevel = GameManager.Instance.CurrentLevel;
        m_animator.runtimeAnimatorController = GameManager.Instance.Get_AlionAnimator(m_imageType);
    }

    private void Update()
    {
        if(m_currentAlienType != GameManager.Instance.CurrentAlienType || m_currentLevel != GameManager.Instance.CurrentLevel)
        {
            m_currentAlienType = GameManager.Instance.CurrentAlienType;
            m_currentLevel = GameManager.Instance.CurrentLevel;
            m_animator.runtimeAnimatorController = GameManager.Instance.Get_AlionAnimator(m_imageType);
        }
    }
}
