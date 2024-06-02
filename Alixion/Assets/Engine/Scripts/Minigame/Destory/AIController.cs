using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public BlockManager aiBlockManager;
    public List<Block> blockList;

    private void Start()
    {
        StartCoroutine(RemoveBlockRoutine());
    }
    private IEnumerator RemoveBlockRoutine()
    {
        while (blockList.Count > 0)
        {
            float randomDelay = Random.Range(1f, 3f); // ������ �ð� (1�ʿ��� 3�� ����)
            yield return new WaitForSeconds(randomDelay);

            Block topBlock = aiBlockManager.GetTopBlock(blockList);
            if (topBlock != null)
            {
                topBlock.DecreaseHealth();
                blockList.Remove(topBlock);
                Destroy(topBlock.gameObject);
                aiBlockManager.CheckVictoryCondition();
            }
        }
    }
}
