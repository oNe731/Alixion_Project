using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour // 플레이어의 터치 한번에 블럭 하나 부서짐
{
    public GameObject destructionSprite; // 파괴 시 표시할 이펙트
    private int m_health = 1; // 블럭의 체력

    public virtual void Decrease_Health(RuinManager.TYPE type)
    {
        m_health--;
        if (m_health <= 0)
            Destroyed_Block(type);
    }

    protected void Destroyed_Block(RuinManager.TYPE type)
    {
        // 블럭 파괴
        RuinManager.Instance.Play_BlockDestroySound();
        RuinManager.Instance.Destroyed_Block(type, this);

        // 파괴 이펙트 생성
        if (destructionSprite != null)
            Instantiate(destructionSprite, transform.position, Quaternion.identity);
    }

    public void Set_Health(int health)
    {
        this.m_health = health;
    }
}
