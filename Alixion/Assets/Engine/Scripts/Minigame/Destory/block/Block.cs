using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    public int health = 1; // 블럭의 체력
    private bool canBeDestroyed = true; // 블럭이 파괴될 수 있는 상태인지 여부

    private void OnMouseDown()
    {
        if (!canBeDestroyed || !BlockManager.Instance.CanRemoveBlock) return;

        Block topBlock = BlockManager.Instance.GetTopBlock();
        if (topBlock != this) return;

        DecreaseHealth();
        BlockManager.Instance.SetCanRemoveBlock(false); // 블럭 제거 후 다음 클릭 전까지 블럭 제거 불가
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
        // 블럭 파괴 시 추가 로직
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

    // SetHealth 메서드 추가
    public void SetHealth(int health)
    {
        this.health = health;
    }
}
