using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;

    public List<GameObject> blockPrefabs; // ���� ���� �� �������� ���� ����Ʈ
    public List<GameObject> specialBlockPrefabs; // ���� ���� ����� �� �������� ���� ����Ʈ
    public Transform blockContainer; // �÷��̾��� ���� ��ġ�� �θ� ������Ʈ
    public Transform ai1BlockContainer; // AI1�� ���� ��ġ�� �θ� ������Ʈ
    public Transform ai2BlockContainer; // AI2�� ���� ��ġ�� �θ� ������Ʈ
    public int initialBlockCount = 50; // �ʱ� �� ����
    public GameObject playerFallingObject; // �÷��̾��� ���� ������Ʈ
    public GameObject ai1FallingObject; // AI1�� ���� ������Ʈ
    public GameObject ai2FallingObject; // AI2�� ���� ������Ʈ
    public Transform ground; // �ٴ� ������Ʈ
    public Vector3 playerFallingObjectStartPosition; // �÷��̾� ���� ������Ʈ�� ���� ��ǥ
    public Vector3 ai1FallingObjectStartPosition; // AI1 ���� ������Ʈ�� ���� ��ǥ
    public Vector3 ai2FallingObjectStartPosition; // AI2 ���� ������Ʈ�� ���� ��ǥ
    public GameObject specialBlockTouchIndicator; // ����� �� ��ġ �� Ȱ��ȭ�� ������Ʈ
    public GameObject playerVictoryUI; // �÷��̾� �¸� UI
    public GameObject playerSecondPlaceUI; // �÷��̾� 2�� UI
    public GameObject playerLoseUI; // �÷��̾� �й� UI
    private float startTime;
    private bool canRemoveBlock = true; // �� ���� ���� ����

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

        // �¸� UI ��Ȱ��ȭ
        if (playerVictoryUI != null) playerVictoryUI.SetActive(false);
        if (playerSecondPlaceUI != null) playerSecondPlaceUI.SetActive(false);
        if (playerLoseUI != null) playerLoseUI.SetActive(false);

        // ���� ������Ʈ ��ġ ����
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

        // AI ����
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
            CanRemoveBlock = true; // ���콺�� ���� �ٽ� �� ���� ����
        }
    }

    private void HandleTouchStart()
    {
        // �� ���� �� �ʱ� ��ġ ��ġ ����
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
        // �� ���� �� ���콺 �� �̺�Ʈ ó��
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
            SetCanRemoveBlock(false); // �� ���� �� ���� Ŭ�� ������ �� ���� �Ұ�
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
        float blockHeight = 1f; // ���� ����
        Vector3 startPosition = container.position;

        for (int i = 0; i < initialBlockCount; i++)
        {
            GameObject block;
            if (Random.value < 0.2f) // 20% Ȯ���� ����� �� ����
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
                rb.bodyType = RigidbodyType2D.Kinematic; // ��ġ ������ ���� Kinematic���� ����
            }

            blockScript.SetHealth(1);

            // ���� ��ġ ���� (���� ���̵���)
            block.transform.localPosition = startPosition + new Vector3(0, i * blockHeight, 0);

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // ��ġ ���� �� Dynamic���� ����
            }
        }
    }

    private IEnumerator AIRemoveBlockRoutine(List<Block> blockList, Transform container)
    {
        while (blockList.Count > 0)
        {
            float randomDelay = Random.Range(1f, 3f); // ������ �ð� (1�ʿ��� 3�� ����)
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
        yield return new WaitForSeconds(1f); // 1�� ��� �� �¸� UI ǥ��
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
                SetCanRemoveBlock(false); // �� ���� �� ���� Ŭ�� ������ �� ���� �Ұ�
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
        // Ŭ���� ���� ���� �߰�
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
