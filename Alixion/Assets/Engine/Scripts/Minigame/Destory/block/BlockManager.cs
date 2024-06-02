using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;

    public List<GameObject> blockPrefabs; // 여러 개의 블럭 프리팹을 받을 리스트
    public List<GameObject> specialBlockPrefabs; // 여러 개의 스페셜 블럭 프리팹을 받을 리스트
    public Transform blockContainer; // 플레이어의 블럭을 배치할 부모 오브젝트
    public Transform ai1BlockContainer; // AI1의 블럭을 배치할 부모 오브젝트
    public Transform ai2BlockContainer; // AI2의 블럭을 배치할 부모 오브젝트
    public int initialBlockCount = 50; // 초기 블럭 개수
    public GameObject playerFallingObject; // 플레이어의 폴링 오브젝트
    public GameObject ai1FallingObject; // AI1의 폴링 오브젝트
    public GameObject ai2FallingObject; // AI2의 폴링 오브젝트
    public Transform ground; // 바닥 오브젝트
    public Vector3 playerFallingObjectStartPosition; // 플레이어 폴링 오브젝트의 시작 좌표
    public Vector3 ai1FallingObjectStartPosition; // AI1 폴링 오브젝트의 시작 좌표
    public Vector3 ai2FallingObjectStartPosition; // AI2 폴링 오브젝트의 시작 좌표
    public GameObject specialBlockTouchIndicator; // 스페셜 블럭 터치 시 활성화될 오브젝트
    public GameObject playerVictoryUI; // 플레이어 승리 UI
    public GameObject playerSecondPlaceUI; // 플레이어 2등 UI
    public GameObject playerLoseUI; // 플레이어 패배 UI
    private float startTime;
    private bool canRemoveBlock = true; // 블럭 제거 가능 여부

    public bool CanRemoveBlock
    {
        get { return canRemoveBlock; }
        private set { canRemoveBlock = value; }
    }

    public int PlayerRemovedBlocks { get; private set; } = 0;

    private List<Block> playerBlocks = new List<Block>();
    private List<Block> ai1Blocks = new List<Block>();
    private List<Block> ai2Blocks = new List<Block>();
    private int ai1RemovedBlocks = 0;
    private int ai2RemovedBlocks = 0;
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateBlocks(blockContainer, playerBlocks);
        GenerateBlocks(ai1BlockContainer, ai1Blocks);
        GenerateBlocks(ai2BlockContainer, ai2Blocks);
        startTime = Time.time;

        // 승리 UI 비활성화
        if (playerVictoryUI != null) playerVictoryUI.SetActive(false);
        if (playerSecondPlaceUI != null) playerSecondPlaceUI.SetActive(false);
        if (playerLoseUI != null) playerLoseUI.SetActive(false);

        // 폴링 오브젝트 위치 설정
        if (playerFallingObject != null)
        {
            playerFallingObject.transform.position = playerFallingObjectStartPosition;
        }
        if (ai1FallingObject != null)
        {
            ai1FallingObject.transform.position = ai1FallingObjectStartPosition;
        }
        if (ai2FallingObject != null)
        {
            ai2FallingObject.transform.position = ai2FallingObjectStartPosition;
        }

        // AI 설정
        StartCoroutine(AIRemoveBlockRoutine(ai1Blocks, ai1BlockContainer));
        StartCoroutine(AIRemoveBlockRoutine(ai2Blocks, ai2BlockContainer));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanRemoveBlock)
        {
            HandleTouchStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouchEnd();
            CanRemoveBlock = true; // 마우스를 떼면 다시 블럭 제거 가능
        }
    }

    private void HandleTouchStart()
    {
        // 블럭 선택 시 초기 터치 위치 설정
        foreach (Block block in playerBlocks)
        {
            if (block is SpecialBlock specialBlock)
            {
                specialBlock.OnMouseDown();
            }
        }
    }

    private void HandleTouchEnd()
    {
        // 블럭 선택 시 마우스 업 이벤트 처리
        foreach (Block block in playerBlocks)
        {
            if (block is SpecialBlock specialBlock)
            {
                specialBlock.OnMouseUp();
            }
        }

        Block topBlock = GetTopBlock(playerBlocks);
        if (topBlock != null && !(topBlock is SpecialBlock))
        {
            topBlock.DecreaseHealth();
            Debug.Log("Block removed by touch!");
            SetCanRemoveBlock(false); // 블럭 제거 후 다음 클릭 전까지 블럭 제거 불가
            PlayerRemovedBlocks++;
            CheckVictoryCondition();
        }
    }

    public void SetCanRemoveBlock(bool value)
    {
        CanRemoveBlock = value;
    }

    private void GenerateBlocks(Transform container, List<Block> blockList)
    {
        float blockHeight = 1f; // 블럭의 높이
        Vector3 startPosition = container.position;

        for (int i = 0; i < initialBlockCount; i++)
        {
            GameObject block;
            if (Random.value < 0.2f) // 20% 확률로 스페셜 블럭 생성
            {
                block = Instantiate(specialBlockPrefabs[Random.Range(0, specialBlockPrefabs.Count)], container);
            }
            else
            {
                block = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Count)], container);
            }

            blockList.Add(block.GetComponent<Block>());

            Block blockScript = block.GetComponent<Block>();
            Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // 위치 설정을 위해 Kinematic으로 설정
            }

            blockScript.SetHealth(1);

            // 블럭의 위치 설정 (위로 쌓이도록)
            block.transform.localPosition = startPosition + new Vector3(0, i * blockHeight, 0);

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // 위치 설정 후 Dynamic으로 변경
            }
        }
    }

    private IEnumerator AIRemoveBlockRoutine(List<Block> blockList, Transform container)
    {
        while (blockList.Count > 0)
        {
            float randomDelay = Random.Range(1f, 3f); // 랜덤한 시간 (1초에서 3초 사이)
            yield return new WaitForSeconds(randomDelay);

            Block topBlock = GetTopBlock(blockList);
            if (topBlock != null)
            {
                topBlock.DecreaseHealth();
                if (container == ai1BlockContainer)
                {
                    ai1RemovedBlocks++;
                }
                else if (container == ai2BlockContainer)
                {
                    ai2RemovedBlocks++;
                }
                blockList.Remove(topBlock);
                Destroy(topBlock.gameObject);
                CheckVictoryCondition();
            }
        }
    }

    public void BlockDestroyed(Block block)
    {
        playerBlocks.Remove(block);
        Destroy(block.gameObject);

        if (playerBlocks.Count == 0)
        {
            StartCoroutine(ShowVictoryUI());
        }
    }

    private IEnumerator ShowVictoryUI()
    {
        yield return new WaitForSeconds(1f); // 1초 대기 후 승리 UI 표시
        if (!gameEnded)
        {
            gameEnded = true;
            if (PlayerRemovedBlocks > ai1RemovedBlocks && PlayerRemovedBlocks > ai2RemovedBlocks)
            {
                if (playerVictoryUI != null) playerVictoryUI.SetActive(true);
            }
            else if (PlayerRemovedBlocks > ai1RemovedBlocks || PlayerRemovedBlocks > ai2RemovedBlocks)
            {
                if (playerSecondPlaceUI != null) playerSecondPlaceUI.SetActive(true);
            }
            else
            {
                if (playerLoseUI != null) playerLoseUI.SetActive(true);
            }
        }
    }

    public void CheckVictoryCondition()
    {
        if (playerBlocks.Count == 0)
        {
            StartCoroutine(ShowVictoryUI());
        }
    }

    public Block GetTopBlock(List<Block> blockList = null)
    {
        if (blockList == null)
        {
            blockList = playerBlocks;
        }

        if (blockList.Count == 0) return null;
        Block topBlock = blockList[0];
        foreach (Block block in blockList)
        {
            if (block.transform.position.y > topBlock.transform.position.y)
            {
                topBlock = block;
            }
        }
        return topBlock;
    }

    public void RemoveTopBlock()
    {
        if (CanRemoveBlock)
        {
            Block topBlock = GetTopBlock(playerBlocks);
            if (topBlock != null)
            {
                topBlock.DecreaseHealth();
                Debug.Log("Block removed by touch!");
                SetCanRemoveBlock(false); // 블럭 제거 후 다음 클릭 전까지 블럭 제거 불가
                PlayerRemovedBlocks++;
                CheckVictoryCondition();
            }
        }
    }

    private IEnumerator CheckFallingObject()
    {
        while (playerFallingObject.transform.position.y > ground.position.y)
        {
            yield return null;
        }
        GameOver();
    }

    private void GameOver()
    {
        float elapsedTime = Time.time - startTime;
        Debug.Log("Game Over! Time: " + elapsedTime + " seconds.");
        // 클리어 보상 로직 추가
    }

    public void IncreaseNextBlocksHealth(int count)
    {
        int incrementedCount = 0;
        for (int i = 0; i < playerBlocks.Count && incrementedCount < count; i++)
        {
            Block block = playerBlocks[i];
            if (!(block is SpecialBlock))
            {
                block.SetHealth(2);
                incrementedCount++;
            }
        }
    }
}
