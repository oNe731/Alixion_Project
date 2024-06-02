using System.Collections;
using UnityEngine;

public class HealthIncreaseSpecialBlock : SpecialBlock
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
        Debug.Log("HealthIncreaseSpecialBlock touched incorrectly!");

        // 스페셜 블럭을 제외한 아래의 최상위 5개 블럭의 체력을 2로 설정
        BlockManager.Instance.IncreaseNextBlocksHealth(5);

        // 특별한 오브젝트 활성화
        if (specialEffect != null)
        {
            specialEffect.SetActive(true);
            StartCoroutine(DeactivateSpecialEffect(5)); // 5개의 블럭 제거할 동안 활성화
        }
    }

    private IEnumerator DeactivateSpecialEffect(int blockCount)
    {
        for (int i = 0; i < blockCount; i++)
        {
            yield return new WaitUntil(() => BlockManager.Instance.PlayerRemovedBlocks > i);
        }
        if (specialEffect != null)
        {
            specialEffect.SetActive(false);
        }
    }
}
