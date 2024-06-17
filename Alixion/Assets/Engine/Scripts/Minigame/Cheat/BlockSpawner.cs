namespace Fraud
{
    using UnityEngine;
    using System.Collections.Generic;

    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_blockPrefabs;
        [SerializeField] private Vector2 m_spawnAreaMin = new Vector2(-2f, 0f);
        [SerializeField] private Vector2 m_spawnAreaMax = new Vector2(1.6f, 4f);

        private List<Bounds> m_blockBoundsList = new List<Bounds>();

        public void Spawn_Blocks(int BlockCount)
        {
            m_blockBoundsList.Clear();

            for (int i = 0; i < BlockCount; i++)
            {
                GameObject tempBlock = null;

                // ������ ��ġ�� ã�� ���� �õ� Ƚ��
                int attempts = 0;
                bool positionFound = false;
                while (attempts < 100 && !positionFound)
                {
                    // �ӽ÷� ���� ������ ��ġ�� ������ Bounds�� Ȯ��
                    Vector2 randomPosition = new Vector2(Random.Range(m_spawnAreaMin.x, m_spawnAreaMax.x), Random.Range(m_spawnAreaMin.y, m_spawnAreaMax.y));
                    tempBlock = Instantiate(m_blockPrefabs[Random.Range(0, m_blockPrefabs.Length)], randomPosition, Quaternion.identity);
                    Bounds tempBounds = tempBlock.GetComponent<Renderer>().bounds;

                    // ��ġ���� Ȯ��
                    bool isOverlapping = false;
                    foreach (Bounds bounds in m_blockBoundsList)
                    {
                        if (tempBounds.Intersects(bounds))
                        {
                            isOverlapping = true;
                            break;
                        }
                    }

                    // ��ġ�� �ʴ� ��ġ��� �� ���� Ȯ��
                    if (isOverlapping == false)
                    {
                        m_blockBoundsList.Add(tempBounds);
                        positionFound = true;
                    }
                    else
                    {
                        Destroy(tempBlock);
                    }

                    attempts++;
                }

                // ��ġ�� 100�� �ȿ� ã�� ����
                if (positionFound == false && tempBlock != null)
                    Destroy(tempBlock);
            }
        }
    }
}

