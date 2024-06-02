using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    public int health = 1; // ���� ü��
    private bool canBeDestroyed = true; // ���� �ı��� �� �ִ� �������� ����

    private void OnMouseDown()
    {
        if (!canBeDestroyed || !BlockManager.Instance.CanRemoveBlock) return;

        Block topBlock = BlockManager.Instance.GetTopBlock();
        if (topBlock != this) return;

        DecreaseHealth();
        BlockManager.Instance.SetCanRemoveBlock(false); // �� ���� �� ���� Ŭ�� ������ �� ���� �Ұ�
    }

    public virtual void DecreaseHealth()
    {
        health--;
        if (health <= 0)
        {
            DestroyBlock();
        }
    }

    protected void DestroyBlock()
    {
        // �� �ı� �� �߰� ����
        BlockManager.Instance.BlockDestroyed(this);
        Destroy(gameObject);
    }

    public void PauseDestruction(float seconds)
    {
        StartCoroutine(PauseCoroutine(seconds));
    }

    private IEnumerator PauseCoroutine(float seconds)
    {
        canBeDestroyed = false;
        yield return new WaitForSeconds(seconds);
        canBeDestroyed = true;
    }

    // SetHealth �޼��� �߰�
    public void SetHealth(int health)
    {
        this.health = health;
    }
}
