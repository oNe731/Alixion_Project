using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    // 프리팹으로 지정할 블럭들
    public GameObject[] blockPrefabs;

    // 블럭이 생성될 범위 설정 (사각형 영역)
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    // 생성할 블럭의 수
    public int numberOfBlocks = 10;

    // 기존 블럭들의 위치를 저장할 리스트
    private List<Bounds> blockBoundsList = new List<Bounds>();

    void Start()
    {
        SpawnBlocks();
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < numberOfBlocks; i++)
        {
            // 무작위 위치를 찾기 위한 시도 횟수
            int attempts = 0;
            bool positionFound = false;

            while (attempts < 100 && !positionFound)
            {
                // 무작위 위치 생성
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                Vector2 randomPosition = new Vector2(randomX, randomY);

                // 무작위 블럭 프리팹 선택
                int randomIndex = Random.Range(0, blockPrefabs.Length);
                GameObject selectedPrefab = blockPrefabs[randomIndex];

                // 임시로 블럭을 생성해 Bounds를 확인
                GameObject tempBlock = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
                Bounds tempBounds = tempBlock.GetComponent<Renderer>().bounds;

                // 겹치는지 확인
                bool isOverlapping = false;
                foreach (Bounds bounds in blockBoundsList)
                {
                    if (tempBounds.Intersects(bounds))
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                // 겹치지 않는 위치라면 블럭 생성 확정
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

            // 만약 위치를 찾지 못했으면, 경고 메시지 출력
            if (!positionFound)
            {
                Debug.LogWarning("블럭을 놓을 수 있는 위치를 찾지 못했습니다.");
            }
        }
    }
}
