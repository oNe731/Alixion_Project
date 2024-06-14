using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuinManager : LevelManager
{
    public enum TYPE { TYPE_AI1, TYPE_PLAYER, TYPE_AI2, TYPE_END }

    [SerializeField] private int m_initialBlockCount = 50;
    [SerializeField] private List<GameObject> m_blockPrefabs;
    [SerializeField] private List<GameObject> m_specialBlockPrefabs;
    [SerializeField] private Transform[] m_blockContainer;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_finishPanel;

    private static RuinManager m_instance;
    private List<List<Block>> m_blocks = new List<List<Block>>();
    private bool m_canRemoveBlock = true;
    private string[] m_places = new string[3];
    private int m_placeCounter = 0;
    private AudioSource m_audioSource;

    private List<GameObject> m_increasedHealthBlocks = new List<GameObject>();

    public static RuinManager Instance => m_instance;
    public GameObject Player => m_player;
    public bool CanRemoveBlock
    {
        get { return m_canRemoveBlock; }
        set { m_canRemoveBlock = value; }
    }


    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Main.UIManager.Instance.Start_FadeIn(0.5f, Color.black);

        m_audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < (int)TYPE.TYPE_END; ++i)
            m_blocks.Add(new List<Block>());

        Generate_Blocks(TYPE.TYPE_PLAYER, m_blockContainer[(int)TYPE.TYPE_PLAYER], m_blocks[(int)TYPE.TYPE_PLAYER]);
        Generate_Blocks(TYPE.TYPE_AI1, m_blockContainer[(int)TYPE.TYPE_AI1], m_blocks[(int)TYPE.TYPE_AI1]);
        Generate_Blocks(TYPE.TYPE_AI2, m_blockContainer[(int)TYPE.TYPE_AI2], m_blocks[(int)TYPE.TYPE_AI2]);
    }

    private void Generate_Blocks(TYPE type, Transform container, List<Block> blockList)
    {
        float blockHeight = 1f;
        Vector3 startPosition = container.position;

        for (int i = 0; i < m_initialBlockCount; i++)
        {
            GameObject block;
            if (Random.value < 0.2f)
                block = Instantiate(m_specialBlockPrefabs[Random.Range(0, m_specialBlockPrefabs.Count)], container);
            else
                block = Instantiate(m_blockPrefabs[Random.Range(0, m_blockPrefabs.Count)], container);
            block.transform.localPosition = startPosition + new Vector3(0, i * blockHeight, 0);

            Block blockScript = block.GetComponent<Block>();
            blockScript.Set_Health(1);
            blockList.Add(blockScript);

            Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public override void Start_Game()
    {
        Camera.main.gameObject.GetComponent<AudioSource>().Play();
        GameManager.Instance.IsMiniGame = true;

        StartCoroutine(Start_AI(TYPE.TYPE_AI1, m_blocks[(int)TYPE.TYPE_AI1]));
        StartCoroutine(Start_AI(TYPE.TYPE_AI2, m_blocks[(int)TYPE.TYPE_AI2]));
    }

    private IEnumerator Start_AI(TYPE type, List<Block> blockList)
    {
        while (blockList.Count > 0)
        {
            if (GameManager.Instance.IsMiniGame == false)
                break;

            if (GameManager.Instance.Pause == false)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

                Block topBlock = Get_TopBlock(blockList);
                if (topBlock != null)
                    topBlock.Decrease_Health(type);
                else
                    break;
            }

            yield return null;
        }
    }



    private void Update()
    {
        if (GameManager.Instance.IsMiniGame == false || CanRemoveBlock == false || GameManager.Instance.Pause == true) 
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Start_HandleTouch(touch.position);
                    break;
                case TouchPhase.Ended:
                    End_HandleTouch(touch.position);
                    break;
            }
        }
    }

    public void Start_HandleTouch(Vector2 touchPosition)
    {
        Block topBlock = Get_TopBlock(m_blocks[(int)TYPE.TYPE_PLAYER]);
        if (topBlock != null && topBlock is SpecialBlock healthBlock)
            healthBlock.Start_OnTouch(touchPosition);
    }

    public void End_HandleTouch(Vector2 touchPosition)
    {
        Block topBlock = Get_TopBlock(m_blocks[(int)TYPE.TYPE_PLAYER]);
        if (topBlock != null)
        {
            if (topBlock is SpecialBlock healthBlock)
                healthBlock.End_OnTouch(touchPosition);
            else
                topBlock.Decrease_Health(TYPE.TYPE_PLAYER);
        }
    }



    public Block Get_TopBlock(List<Block> blockList = null)
    {
        if (blockList == null)
            blockList = m_blocks[(int)TYPE.TYPE_PLAYER];

        if (blockList.Count == 0) 
            return null;

        Block topBlock = blockList[0];
        foreach (Block block in blockList)
        {
            if (block.transform.position.y > topBlock.transform.position.y)
                topBlock = block;
        }

        return topBlock;
    }

    public void Destroyed_Block(TYPE type, Block block)
    {
        if (type == TYPE .TYPE_PLAYER && m_increasedHealthBlocks.Count > 0)
            Check_Block(block);

        m_blocks[(int)type].Remove(block);
        Destroy(block.gameObject);
    }


    public void Mistake_HealthIncreaseSpecialBlock()
    {
        m_player.transform.GetChild(0).gameObject.SetActive(true);
        Increase_NextBlocksHealth(5);
    }

    private void Increase_NextBlocksHealth(int count)
    {
        m_increasedHealthBlocks.Clear();

        int incrementedCount = 0;
        for (int i = m_blocks[(int)TYPE.TYPE_PLAYER].Count - 1; i >= 0 && incrementedCount < count; i--)
        {
            Block block = m_blocks[(int)TYPE.TYPE_PLAYER][i];
            if (block is SpecialBlock == false)
            {
                block.Set_Health(2);
                m_increasedHealthBlocks.Add(block.gameObject);
                incrementedCount++;
            }
        }
    }

    private void Check_Block(Block block)
    {
        m_increasedHealthBlocks.Remove(block.gameObject);

        if (m_increasedHealthBlocks.Count == 0)
            RuinManager.Instance.Player.transform.GetChild(0).gameObject.SetActive(false);
    }


    public void Check_GameOver(string playerTag)
    {
        m_places[m_placeCounter] = playerTag;
        m_placeCounter++;

        if (playerTag == "Player") // 플레이어가 블럭을 다 파괴하고 바닥에 도달했을 때
        {
            GameManager.Instance.IsMiniGame = false;

            m_finishPanel.SetActive(true);
            m_finishPanel.GetComponent<FinishPanel>().Finish_Game(FinishPanel.FinishType.FT_CLEAR);
            if (m_places[0] == "Player")
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(3, "UI_Item_Ruin1", "UI_Item_Ruin2", "UI_Item_Ruin3");
            else if (m_places[1] == "Player")
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(2, "UI_Item_Ruin1", "UI_Item_Ruin2");
            else if (m_places[2] == "Player")
                m_finishPanel.GetComponent<FinishPanel>().Create_Item(1, "UI_Item_Ruin1");
        }
    }

    public void Play_BlockDestroySound()
    {
        if (m_audioSource == null)
            return;

        m_audioSource.PlayOneShot(Resources.Load<AudioClip>("Sonds/Effect/MiniGame/CheDes_Game/destory"));
    }
}
