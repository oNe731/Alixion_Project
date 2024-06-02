using System.Collections;
using UnityEngine;

public class TouchDisableSpecialBlock : SpecialBlock
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
        Debug.Log("TouchDisableSpecialBlock touched incorrectly!");

        // ��ġ �Է��� 5�ʰ� ����
        BlockManager.Instance.SetCanRemoveBlock(false);

        // Ư���� ������Ʈ Ȱ��ȭ
        if (specialEffect != null)
        {
            specialEffect.SetActive(true);
            StartCoroutine(DeactivateSpecialEffect(5f)); // 5�� ���� Ȱ��ȭ
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
