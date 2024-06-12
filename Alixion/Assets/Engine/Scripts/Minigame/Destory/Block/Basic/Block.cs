using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour // �÷��̾��� ��ġ �ѹ��� �� �ϳ� �μ���
{
    public GameObject destructionSprite; // �ı� �� ǥ���� ����Ʈ
    private int m_health = 1; // ���� ü��

    public virtual void Decrease_Health(RuinManager.TYPE type)
    {
        m_health--;
        if (m_health <= 0)
            Destroyed_Block(type);
    }

    protected void Destroyed_Block(RuinManager.TYPE type)
    {
        // �� �ı�
        RuinManager.Instance.Play_BlockDestroySound();
        RuinManager.Instance.Destroyed_Block(type, this);

        // �ı� ����Ʈ ����
        if (destructionSprite != null)
            Instantiate(destructionSprite, transform.position, Quaternion.identity);
    }

    public void Set_Health(int health)
    {
        this.m_health = health;
    }
}
