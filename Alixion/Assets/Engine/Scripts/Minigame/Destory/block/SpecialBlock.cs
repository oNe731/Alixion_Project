using System.Collections;
using UnityEngine;

public class SpecialBlock : Block
{
    public GameObject touchIndicator; // 터치 시 활성화될 오브젝트
    private Vector2 initialTouchPosition;
    private bool isSwipe;

    public void OnMouseDown()
    {
        if (!BlockManager.Instance.CanRemoveBlock) return;

        initialTouchPosition = Input.mousePosition;
        isSwipe = false;
    }

    public void OnMouseDrag()
    {
        if (!BlockManager.Instance.CanRemoveBlock) return;

        if (Vector2.Distance(initialTouchPosition, Input.mousePosition) > 50f) // 슬라이드 거리 임계값 설정
        {
            isSwipe = true;
        }
    }

    public void OnMouseUp()
    {
        if (!BlockManager.Instance.CanRemoveBlock) return;

        if (isSwipe)
        {
            // 슬라이드로 인식된 경우
            Debug.Log("Special block removed by swipe!");
            DecreaseHealth();
        }
        else
        {
            // 클릭으로 인식된 경우
            Debug.Log("Special block touched incorrectly!");
            OnMistakeTouch();
        }
    }

    public override void DecreaseHealth()
    {
        base.DecreaseHealth();
        if (health <= 0)
        {
            BlockManager.Instance.BlockDestroyed(this);
        }
    }

    public virtual void OnMistakeTouch()
    {
        if (touchIndicator != null)
        {
            touchIndicator.SetActive(true);
            StartCoroutine(DeactivateTouchIndicator());
        }
    }

    private IEnumerator DeactivateTouchIndicator()
    {
        yield return new WaitForSeconds(1f);
        if (touchIndicator != null)
        {
            touchIndicator.SetActive(false);
        }
    }
}
