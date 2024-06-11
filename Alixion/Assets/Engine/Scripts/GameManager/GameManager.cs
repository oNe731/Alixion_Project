using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 우선 순위 : 파멸 > 선 > 사기 > 은둔 > 광기 
public enum PROPERTYTYPE { PT_RUIN, PT_ZEN, PT_FRAUD, PT_SECLUSION, PT_MADNESS, PT_END };
public enum IMAGETYPE { IT_SPRITE, IT_IMAGE, IT_END };

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    [SerializeField] private GameObject m_profilePanel;
    [SerializeField] private GameObject m_inventoryPanel;
    [SerializeField] private GameObject m_inventoryItemPanel;
    [SerializeField] private GameObject m_inventoryPopupPanel;
    [SerializeField] private GameObject m_encyclopediaPanel;
    [SerializeField] private GameObject m_settingPanel;

    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_BASIC;
    private int m_currentLevel = 0;

    private int m_sumPoint = 0;
    private int[] m_alienPoint;
    private Inventory m_inventory;
    private Encyclopedia m_encyclopedia;
    private List<AlienData> alienDatas;
    private bool m_tutorial = false;

    private bool m_isMiniGame = false;
    private bool m_pause = false;

    public ALIENTYPE CurrentAlienType => m_currentAlienType;
    public int CurrentLevel => m_currentLevel;
    public int SumPoint => m_sumPoint;
    public GameObject InventoryPanel => m_inventoryPanel;
    public Inventory Inventory => m_inventory;
    public GameObject EncyclopediaPanel => m_encyclopediaPanel;
    public Encyclopedia Encyclopedia => m_encyclopedia;
    public bool Tutorial
    {
        get => m_tutorial;
        set => m_tutorial = value;
    }
    public bool IsMiniGame
    {
        get => m_isMiniGame;
        set => m_isMiniGame = value;
    }
    public bool Pause
    {
        get => m_pause;
        set => m_pause = value;
    }

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);

            m_alienPoint = new int[(int)PROPERTYTYPE.PT_END];
            for (int i = 0; i < m_alienPoint.Length; ++i)
                m_alienPoint[i] = 0;
            m_inventory = GetComponent<Inventory>();
            m_encyclopedia = GetComponent<Encyclopedia>();

            alienDatas = new List<AlienData>();
            alienDatas.Add(new AlienData(ALIENTYPE.AT_BASIC, "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic",
                "", "", ""));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN, 
                "UnMixedType/Ruin/AC_Ruin1", "UnMixedType/Ruin/AC_Ruin2", "UnMixedType/Ruin/AC_Ruin3",
                "파괴", 
                "약 50퍼센트...즉 내 힘의 절반 정도만 내면 널 우주의 먼지로 만들어 버릴 수 있어...\n< 지옥 그 이상의 공포를 보여드리죠. >",
                "Ruin + Ruin Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN, 
                "UnMixedType/Zen/AC_Zen1", "UnMixedType/Zen/AC_Zen2", "UnMixedType/Zen/AC_Zen3",
                "선", 
                "새로운 성인의 탄생이다!!\n< 그는 신이야... >",
                "Zen + Zen Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD, 
                "UnMixedType/Fraud/AC_Fraud1", "UnMixedType/Fraud/AC_Fraud2", "UnMixedType/Fraud/AC_Fraud3",
                "사기",
                "내가 팔지못하는 것은 없다. 돈은 항상 옳다!\n< 그는 희대의 사기꾼이야. >",
                "Fraud + Fraud Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION, 
                "UnMixedType/Seclusion/AC_Seclusion1", "UnMixedType/Seclusion/AC_Seclusion2", "UnMixedType/Seclusion/AC_Seclusion3",
                "은둔", 
                "족쇄는 끊어져야 하고 모든 인간은 자유로워져야 한다.\n< 우리는 빛을 섬기며 어둠 속에서 움직인다. >",
                "Seclusion + Seclusion Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_MADNESS, 
                "UnMixedType/Madness/AC_Madness1", "UnMixedType/Madness/AC_Madness2", "UnMixedType/Madness/AC_Madness3",
                "광기", 
                "내 신념은 말이지, 죽을 만큼의 고난은 사람을... '광'하게 만든다는 거야\n< 왜 그리 심각해? > ",
                "Madness + Madness Ending"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_RUIN, 
                "MixedType/Zen+Ruin/AC_Zen_Ruin1", "MixedType/Zen+Ruin/AC_Zen_Ruin2", "MixedType/Zen+Ruin/AC_Zen_Ruin3",
                "선 + 파괴", 
                "이 곳에 있는 인간은 너무나도  더러운 종족이다..\n이렇게 많은 인간은 필요가 없어.\n< 쓰레기는 제거해야해, 난 필연적인 존재다. >",
                "Zen + Ruin Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_FRAUD, 
                "MixedType/Zen+Fraud/AC_Zen_Fraud1", "MixedType/Zen+Fraud/AC_Zen_Fraud2", "MixedType/Zen+Fraud/AC_Zen_Fraud3",
                "선 + 사기", 
                "거짓은 고한 적이 없다 다만 모든 진실을 말하지 않아 사죄하다.\n< 나는 교황이다 인간의 존속은 반드시 필요하다. >",
                "Zen + Fraud Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_SECLUSION, 
                "MixedType/Zen+Seclusion/AC_Zen_Seclusion1", "MixedType/Zen+Seclusion/AC_Zen_Seclusion2", "MixedType/Zen+Seclusion/AC_Zen_Seclusion3",
                "선 + 은둔",
                "교황님 항상 제가 지켜드리겠습니다.\n< 당신은 내마음의 100점 ❤️ >",
                "Zen + Seclusion Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_MADNESS, 
                "MixedType/Zen+Madness/AC_Zen_Madness1", "MixedType/Zen+Madness/AC_Zen_Madness2", "MixedType/Zen+Madness/AC_Zen_Madness3",
                "선 + 광기",
                "이 기도를 주 예수의 이름으로 진심을 담아 드립니다.\n< 폭력을 휘둘러도 되는 대상은 이교도와 괴물 놈들 뿐. >",
                "Zen + Madness Ending"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_FRAUD, 
                "MixedType/Ruin+Fraud/AC_Ruin_Fraud1", "MixedType/Ruin+Fraud/AC_Ruin_Fraud2", "MixedType/Ruin+Fraud/AC_Ruin_Fraud3",
                "파괴 + 사기",
                "전쟁이 곧 나 이니라  전쟁을 하거라.\n미친놈은 미친놈이 잡아야지 내손에 피를 묻히진 않는다.\n< 분열하여 지배하라. >",
                "Ruin + Fraud Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_SECLUSION, 
                "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion1", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion2", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion3",
                 "파괴 + 은둔",
                 "몸과 마음이 우주를 향해서 아무런 까닭없이 번쩍하며 펼쳐내는 것.\n그것이 폭발이다.\n< 예술은 폭발이다!!! >",
                 "Ruin + Seclusion Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_MADNESS, 
                "MixedType/Ruin+Madness/AC_Ruin_Madness1", "MixedType/Ruin+Madness/AC_Ruin_Madness2", "MixedType/Ruin+Madness/AC_Ruin_Madness3",
                "파괴 + 광기",
                "겪어보지 못한 자에게 전쟁이란 달콤한 것이다.\n< 혼란하다! 혼란해! 하하하.>",
                "Ruin + Madness Ending"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_SECLUSION, 
                "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion1", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion2", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion3",
                "사기 + 은둔", 
                "뭔가를 캐내려고 하지 마라... 답하지 않을 터이니...\n< 한 누군가의 뜻대로 세계가 움직인다. >",
                "Fraud + Seclusion Ending"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_MADNESS, 
                "MixedType/Fraud+Madness/AC_Fraud_Madness1", "MixedType/Fraud+Madness/AC_Fraud_Madness2", "MixedType/Fraud+Madness/AC_Fraud_Madness3",
                "사기 + 광기", 
                "나를 믿지 않으면 생명책에서 지워지도록 만들겠다.\n< 나는 신이다. >",
                "Fraud + Madness Ending"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION_MADNESS, 
                "MixedType/Seclusion+Madness/AC_Seclusion_Madness1", "MixedType/Seclusion+Madness/AC_Seclusion_Madness2", "MixedType/Seclusion+Madness/AC_Seclusion_Madness3",
                "은둔 + 광기",
                "너를 죽이려는 것들은 모조리 죽여버리면 돼!!\n< 괜찮아.너는... 내가 지켜줄게. >",
                "Madness + Seclusion Ending"));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #region EVOLUTION
    public void Add_Point(PROPERTYTYPE type, int pointValue)
    {
        Close_InventoryPopup();
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/Item_Use");
        GetComponent<AudioSource>().Play();

        m_alienPoint[(int)type] += pointValue;
        Update_Alien();
    }

    public void Update_Alien()
    {
        if (m_currentLevel == 3) return;

        // 일정 수치마다 외관 변화
        bool update = false;

        m_sumPoint = 0;
        for (int i = 0; i < m_alienPoint.Length; ++i)
            m_sumPoint += m_alienPoint[i];

        if (m_sumPoint >= 45)       { m_currentLevel = 3; update = true; } // 3단계
        else if(m_sumPoint >= 30)   { m_currentLevel = 2; update = true; } // 2단계
        else if (m_sumPoint >= 15)  { m_currentLevel = 1; update = true; } // 1단계

        if (update == false)
            return;

        if (Is_SinglePoint()) { Single_Priority(); }
        else { Combin_Priority(); }
    }

    private bool Is_SinglePoint()
    {
        int valueCount = 0;
        for (int i = 0; i < m_alienPoint.Length; ++i)
        {
            if (m_alienPoint[i] > 0)
                valueCount++;
        }

        return valueCount == 1; //1이라면 참 반환
    }

    private void Single_Priority()
    {
        // 한가지의 포인트를 제외하고 모두 0포인트인 경우 해당 포인트 외계인으로 진화
        if (m_alienPoint[(int)PROPERTYTYPE.PT_RUIN] > 0)          { Set_Alien(alienDatas[(int)ALIENTYPE.AT_RUIN]);      }
        else if (m_alienPoint[(int)PROPERTYTYPE.PT_ZEN] > 0)      { Set_Alien(alienDatas[(int)ALIENTYPE.AT_ZEN]);       }
        else if(m_alienPoint[(int)PROPERTYTYPE.PT_FRAUD] > 0)     { Set_Alien(alienDatas[(int)ALIENTYPE.AT_FRAUD]);     }
        else if(m_alienPoint[(int)PROPERTYTYPE.PT_SECLUSION] > 0) { Set_Alien(alienDatas[(int)ALIENTYPE.AT_SECLUSION]); }
        else if(m_alienPoint[(int)PROPERTYTYPE.PT_MADNESS] > 0)   { Set_Alien(alienDatas[(int)ALIENTYPE.AT_MADNESS]);   }
    }

    private void Combin_Priority()
    {
        // 포인트가 높은 순서대로 정렬 후 제일 높은 포인트 2개를 골라 해당 조합으로 진화
        PROPERTYTYPE[] sortTypes = Sort_PointPriority();
        PROPERTYTYPE type1 = sortTypes[0];
        PROPERTYTYPE type2 = sortTypes[1];

        if (type1 == PROPERTYTYPE.PT_ZEN && type2 == PROPERTYTYPE.PT_RUIN || 
            type1 == PROPERTYTYPE.PT_RUIN && type2 == PROPERTYTYPE.PT_ZEN)
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_ZEN_RUIN]);

        else if (type1 == PROPERTYTYPE.PT_ZEN && type2 == PROPERTYTYPE.PT_FRAUD || 
            type1 == PROPERTYTYPE.PT_FRAUD && type2 == PROPERTYTYPE.PT_ZEN)
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_ZEN_FRAUD]);

        else if(type1 == PROPERTYTYPE.PT_ZEN && type2 == PROPERTYTYPE.PT_SECLUSION || 
            type1 == PROPERTYTYPE.PT_SECLUSION && type2 == PROPERTYTYPE.PT_ZEN)
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_ZEN_SECLUSION]);

        else if (type1 == PROPERTYTYPE.PT_ZEN && type2 == PROPERTYTYPE.PT_MADNESS || 
            type1 == PROPERTYTYPE.PT_MADNESS && type2 == PROPERTYTYPE.PT_ZEN) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_ZEN_MADNESS]);

        else if (type1 == PROPERTYTYPE.PT_RUIN && type2 == PROPERTYTYPE.PT_FRAUD || 
            type1 == PROPERTYTYPE.PT_FRAUD && type2 == PROPERTYTYPE.PT_RUIN) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_RUIN_FRAUD]);

        else if (type1 == PROPERTYTYPE.PT_RUIN && type2 == PROPERTYTYPE.PT_SECLUSION || 
            type1 == PROPERTYTYPE.PT_SECLUSION && type2 == PROPERTYTYPE.PT_RUIN) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_RUIN_SECLUSION]);

        else if (type1 == PROPERTYTYPE.PT_RUIN && type2 == PROPERTYTYPE.PT_MADNESS || 
            type1 == PROPERTYTYPE.PT_MADNESS && type2 == PROPERTYTYPE.PT_RUIN) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_RUIN_MADNESS]);

        else if (type1 == PROPERTYTYPE.PT_FRAUD && type2 == PROPERTYTYPE.PT_SECLUSION || 
            type1 == PROPERTYTYPE.PT_SECLUSION && type2 == PROPERTYTYPE.PT_FRAUD) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_FRAUD_SECLUSION]); 

        else if (type1 == PROPERTYTYPE.PT_FRAUD && type2 == PROPERTYTYPE.PT_MADNESS || 
            type1 == PROPERTYTYPE.PT_MADNESS && type2 == PROPERTYTYPE.PT_FRAUD) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_FRAUD_MADNESS]);

        else if (type1 == PROPERTYTYPE.PT_SECLUSION && type2 == PROPERTYTYPE.PT_MADNESS || 
            type1 == PROPERTYTYPE.PT_MADNESS && type2 == PROPERTYTYPE.PT_SECLUSION) 
            Set_Alien(alienDatas[(int)ALIENTYPE.AT_SECLUSION_MADNESS]);
    }

    private PROPERTYTYPE[] Sort_PointPriority()
    {
        PROPERTYTYPE[] types = new PROPERTYTYPE[m_alienPoint.Length];
        for (int i = 0; i < types.Length; ++i)
            types[i] = (PROPERTYTYPE)i;

        System.Array.Sort(types, (a, b) => m_alienPoint[(int)b].CompareTo(m_alienPoint[(int)a]));
        return types;
    }

    private void Set_Alien(AlienData alienData)
    {
        m_currentAlienType = alienData.Type;
        if (m_currentLevel == 3) { StartCoroutine(Create_EndingCard(alienData)); }
    }

    public RuntimeAnimatorController Get_AlionAnimator(IMAGETYPE type)
    {
        RuntimeAnimatorController animator = null;
        int level = Mathf.Max(m_currentLevel - 1, 0);

        if (type == IMAGETYPE.IT_SPRITE)
        {
            animator = Resources.Load<RuntimeAnimatorController>("Animation/Alien/SpriteType/" + alienDatas[(int)CurrentAlienType].AnimatrNames[level]);
        }
        else if(type == IMAGETYPE.IT_IMAGE)
        {
            animator = Resources.Load<RuntimeAnimatorController>("Animation/Alien/ImageType/" + alienDatas[(int)CurrentAlienType].AnimatrNames[level]);
        }

        return animator;
    }
    #endregion

    public void Start_WaitLodeScene(ScreenOrientation type, string name)
    {
        m_profilePanel.SetActive(false);
        m_inventoryPanel.SetActive(false);
        m_inventoryPopupPanel.SetActive(false);
        m_encyclopediaPanel.SetActive(false);
        m_settingPanel.SetActive(false);

        StartCoroutine(Wait_LodeScene(type, name));
    }

    public IEnumerator Create_EndingCard(AlienData alienData)
    {
        float time = 0f;
        while(time < 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 카드 생성
        GameObject cardObject = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Encyclopedia/Panel_Encyclopedia_Popup"), m_inventoryPanel.transform);
        cardObject.GetComponent<EncyclopediaCard>().Set_Card(alienData, true);

        // 도감에 해당 카드 추가
        m_encyclopedia.Add_Item(alienData);

        yield break;
    }

    public IEnumerator Wait_LodeScene(ScreenOrientation type, string name)
    {
        Screen.orientation = type;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(name);
        yield break;
    }

    public void Reset_AlienStat()
    {
        m_currentAlienType = ALIENTYPE.AT_BASIC;
        m_currentLevel = 0;
        m_sumPoint = 0;
        for (int i = 0; i < m_alienPoint.Length; ++i)
            m_alienPoint[i] = 0;
    }

    #region BUTTON
    public void Open_Profile()
    {
        if (m_profilePanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_profilePanel.SetActive(true);
    }

    public void Close_Profile()
    {
        if (m_profilePanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_profilePanel.SetActive(false);
    }

    public void Open_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryPanel.SetActive(true);
    }

    public void Close_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryPanel.SetActive(false);
        m_inventory.SelctSlot = null;
    }

    public void Open_InventoryItem(ItemData m_item)
    {
        if (m_inventoryItemPanel == null || Inventory == null || Inventory.Use_ItemBool() == false)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryItemPanel.GetComponent<ItemInfo>().Set_Info(m_item);
        m_inventoryItemPanel.SetActive(true);
    }

    public void Close_InventoryItem()
    {
        if (m_inventoryItemPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryItemPanel.SetActive(false);
    }

    public void Open_InventoryPopup()
    {
        if (m_inventoryPopupPanel == null || Inventory == null || Inventory.Use_ItemBool() == false)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryItemPanel.SetActive(false);
        m_inventoryPopupPanel.SetActive(true);
    }

    public void Close_InventoryPopup()
    {
        if (m_inventoryPopupPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryPopupPanel.SetActive(false);
    }

    public void Open_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_encyclopediaPanel.SetActive(true);
    }

    public void Close_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_encyclopediaPanel.SetActive(false);
    }

    public void Open_Settings()
    {
        if (m_settingPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_settingPanel.SetActive(true);
    }
    
    public void Close_Settings()
    {
        if (m_settingPanel == null)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_settingPanel.SetActive(false);
    }

    public void Exit_Game()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Open_InventoryItemInfo()
    {
        if (m_inventory == null || m_inventory.SelctSlot == null || Inventory.Use_ItemBool() == false)
            return;

        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        Open_InventoryItem(m_inventory.SelctSlot.Item);
    }
    #endregion

    public void Play_Sound(string path)
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(path);
        GetComponent<AudioSource>().Play();
    }

    public void False_Pause()
    {
        StartCoroutine(Wait_Pause());
    }

    private IEnumerator Wait_Pause()
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_pause = false;
        yield break;
    }
}
