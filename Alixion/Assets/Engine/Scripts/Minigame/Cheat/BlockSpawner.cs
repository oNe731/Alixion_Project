using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    // ���������� ������ ����
    public GameObject[] blockPrefabs;

    // ���� ������ ���� ���� (�簢�� ����)
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    // ������ ���� ��
    public int numberOfBlocks = 10;

    // ���� ������ ��ġ�� ������ ����Ʈ
    private List<Bounds> blockBoundsList = new List<Bounds>();

    void Start()
    {
        SpawnBlocks();
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < numberOfBlocks; i++)
        {
            // ������ ��ġ�� ã�� ���� �õ� Ƚ��
            int attempts = 0;
            bool positionFound = false;

            while (attempts < 100 && !positionFound)
            {
                // ������ ��ġ ����
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                Vector2 randomPosition = new Vector2(randomX, randomY);

                // ������ �� ������ ����
                int randomIndex = Random.Range(0, blockPrefabs.Length);
                GameObject selectedPrefab = blockPrefabs[randomIndex];

                // �ӽ÷� ���� ������ Bounds�� Ȯ��
                GameObject tempBlock = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
                Bounds tempBounds = tempBlock.GetComponent<Renderer>().bounds;

                // ��ġ���� Ȯ��
                bool isOverlapping = false;
                foreach (Bounds bounds in blockBoundsList)
                {
                    if (tempBounds.Intersects(bounds))
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                // ��ġ�� �ʴ� ��ġ��� �� ���� Ȯ��
                if (!isOverlapping)
                {
                    blockBoundsList.Add(tempBounds);
                    positionFound = true;
                }
                else
                {
                    Destroy(tempBlock);
                }

                attempts++;
            }

            // ���� ��ġ�� ã�� ��������, ��� �޽��� ���
            if (!positionFound)
            {
                Debug.LogWarning("���� ���� �� �ִ� ��ġ�� ã�� ���߽��ϴ�.");
            }
        }
    }
}
