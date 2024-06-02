using System.Collections;
using UnityEngine;

public class HealthIncreaseSpecialBlock : SpecialBlock
{
    public GameObject specialEffect; // Ȱ��ȭ�� Ư���� ������Ʈ

    private void Start()
    {
        if (specialEffect != null)
        {
            specialEffect.SetActive(false); // ���� ���� �� ��Ȱ��ȭ
        }
    }

    public override void OnMistakeTouch()
    {
        base.OnMistakeTouch();
        Debug.Log("HealthIncreaseSpecialBlock touched incorrectly!");

        // ����� ���� ������ �Ʒ��� �ֻ��� 5�� ���� ü���� 2�� ����
        BlockManager.Instance.IncreaseNextBlocksHealth(5);

        // Ư���� ������Ʈ Ȱ��ȭ
        if (specialEffect != null)
        {
            specialEffect.SetActive(true);
            StartCoroutine(DeactivateSpecialEffect(5)); // 5���� �� ������ ���� Ȱ��ȭ
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
