using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialBlock : Block
{
    protected bool m_isSwipe = false;
    protected float m_swipeThreshold = 50f; // 슬라이드로 인식할 최소 거리
    protected Vector2 m_touchStartPos;
    protected Vector2 m_touchEndPos;

    public void Start_OnTouch(Vector2 touchPosition)
    {
        m_touchStartPos = touchPosition;
    }

    public void End_OnTouch(Vector2 touchPosition)
    {
        m_touchEndPos = touchPosition;

        float distance = Vector2.Distance(m_touchStartPos, m_touchEndPos);
        m_isSwipe = distance >= m_swipeThreshold;

        if (m_isSwipe == true)
            Destroyed_Block(RuinManager.TYPE.TYPE_PLAYER);
        else
            Mistake_OnTouch();
    }

    protected abstract void Mistake_OnTouch();
}
