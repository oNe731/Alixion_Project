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

                // 무작위 위치를 찾기 위한 시도 횟수
                int attempts = 0;
                bool positionFound = false;
                while (attempts < 100 && !positionFound)
                {
                    // 임시로 블럭을 무작위 위치에 생성해 Bounds를 확인
                    Vector2 randomPosition = new Vector2(Random.Range(m_spawnAreaMin.x, m_spawnAreaMax.x), Random.Range(m_spawnAreaMin.y, m_spawnAreaMax.y));
                    tempBlock = Instantiate(m_blockPrefabs[Random.Range(0, m_blockPrefabs.Length)], randomPosition, Quaternion.identity);
                    Bounds tempBounds = tempBlock.GetComponent<Renderer>().bounds;

                    // 겹치는지 확인
                    bool isOverlapping = false;
                    foreach (Bounds bounds in m_blockBoundsList)
                    {
                        if (tempBounds.Intersects(bounds))
                        {
                            isOverlapping = true;
                            break;
                        }
                    }

                    // 겹치지 않는 위치라면 블럭 생성 확정
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

                // 위치를 100번 안에 찾지 못함
                if (positionFound == false && tempBlock != null)
                    Destroy(tempBlock);
            }
        }
    }
}

