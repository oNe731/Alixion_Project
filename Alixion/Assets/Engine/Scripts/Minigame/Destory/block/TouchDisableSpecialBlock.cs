using System.Collections;
using UnityEngine;

public class TouchDisableSpecialBlock : SpecialBlock
{
    public GameObject specialEffect; // 활성화할 특별한 오브젝트

    private void Start()
    {
        if (specialEffect != null)
        {
            specialEffect.SetActive(false); // 게임 시작 시 비활성화
        }
    }

    public override void OnMistakeTouch()
    {
        base.OnMistakeTouch();
        Debug.Log("TouchDisableSpecialBlock touched incorrectly!");

        // 터치 입력을 5초간 정지
        BlockManager.Instance.SetCanRemoveBlock(false);

        // 특별한 오브젝트 활성화
        if (specialEffect != null)
        {
            specialEffect.SetActive(true);
            StartCoroutine(DeactivateSpecialEffect(5f)); // 5초 동안 활성화
        }

        StartCoroutine(EnableTouchAfterDelay(5f));
    }

    private IEnumerator DeactivateSpecialEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (specialEffect != null)
        {
            specialEffect.SetActive(false);
        }
    }

    private IEnumerator EnableTouchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BlockManager.Instance.SetCanRemoveBlock(true);
    }
}
