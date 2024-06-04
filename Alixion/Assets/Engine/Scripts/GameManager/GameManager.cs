using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 우선 순위 : 파멸 > 선 > 사기 > 은둔 > 광기 
public enum PROPERTYTYPE { PT_RUIN, PT_ZEN, PT_FRAUD, PT_SECLUSION, PT_MADNESS, PT_END };
public enum IMAGETYPE { IT_SPRITE, IT_IMAGE, IT_END };

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    [SerializeField] private GameObject m_profilePanel;
    [SerializeField] private GameObject m_inventoryPanel;
    [SerializeField] private GameObject m_inventoryPopupPanel;
    [SerializeField] private GameObject m_encyclopediaPanel;
    [SerializeField] private GameObject m_settingPanel;

    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_BASIC;
    private int m_currentLevel = 0;

    private int m_sumPoint = 0;
    private int[] m_alienPoint;
    private Inventory m_inventory;
    private List<AlienData> alienDatas;

    public ALIENTYPE CurrentAlienType => m_currentAlienType;
    public int CurrentLevel => m_currentLevel;
    public int SumPoint => m_sumPoint;
    public GameObject InventoryPanel => m_inventoryPanel;
    public Inventory Inventory => m_inventory;

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

            alienDatas = new List<AlienData>();
            alienDatas.Add(new AlienData(ALIENTYPE.AT_BASIC, "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN, 
                "UnMixedType/Ruin/AC_Ruin1", "UnMixedType/Ruin/AC_Ruin2", "UnMixedType/Ruin/AC_Ruin3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN, 
                "UnMixedType/Zen/AC_Zen1", "UnMixedType/Zen/AC_Zen2", "UnMixedType/Zen/AC_Zen3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD, 
                "UnMixedType/Fraud/AC_Fraud1", "UnMixedType/Fraud/AC_Fraud2", "UnMixedType/Fraud/AC_Fraud3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION, 
                "UnMixedType/Seclusion/AC_Seclusion1", "UnMixedType/Seclusion/AC_Seclusion2", "UnMixedType/Seclusion/AC_Seclusion3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_MADNESS, 
                "UnMixedType/Madness/AC_Madness1", "UnMixedType/Madness/AC_Madness2", "UnMixedType/Madness/AC_Madness3"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_RUIN, 
                "MixedType/Zen+Ruin/AC_Zen_Ruin1", "MixedType/Zen+Ruin/AC_Zen_Ruin2", "MixedType/Zen+Ruin/AC_Zen_Ruin3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_FRAUD, 
                "MixedType/Zen+Fraud/AC_Zen_Fraud1", "MixedType/Zen+Fraud/AC_Zen_Fraud2", "MixedType/Zen+Fraud/AC_Zen_Fraud3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_SECLUSION, 
                "MixedType/Zen+Seclusion/AC_Zen_Seclusion1", "MixedType/Zen+Seclusion/AC_Zen_Seclusion2", "MixedType/Zen+Seclusion/AC_Zen_Seclusion3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_MADNESS, 
                "MixedType/Zen+Madness/AC_Zen_Madness1", "MixedType/Zen+Madness/AC_Zen_Madness2", "MixedType/Zen+Madness/AC_Zen_Madness3"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_FRAUD, 
                "MixedType/Ruin+Fraud/AC_Ruin_Fraud1", "MixedType/Ruin+Fraud/AC_Ruin_Fraud2", "MixedType/Ruin+Fraud/AC_Ruin_Fraud3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_SECLUSION, 
                "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion1", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion2", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_MADNESS, 
                "MixedType/Ruin+Madness/AC_Ruin_Madness1", "MixedType/Ruin+Madness/AC_Ruin_Madness2", "MixedType/Ruin+Madness/AC_Ruin_Madness3"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_SECLUSION, 
                "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion1", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion2", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion3"));
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_MADNESS, 
                "MixedType/Fraud+Madness/AC_Fraud_Madness1", "MixedType/Fraud+Madness/AC_Fraud_Madness2", "MixedType/Fraud+Madness/AC_Fraud_Madness3"));

            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION_MADNESS, 
                "MixedType/Seclusion+Madness/AC_Seclusion_Madness1", "MixedType/Seclusion+Madness/AC_Seclusion_Madness2", "MixedType/Seclusion+Madness/AC_Seclusion_Madness3"));
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

        if (m_currentLevel == 3)
        {
            // 성장이 끝나면 엔딩과 엔드카드가 나오고 도감에 추가
        }
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

    #region BUTTON
    public void Open_Profile()
    {
        if (m_profilePanel == null)
            return;

        m_profilePanel.SetActive(true);
    }

    public void Close_Profile()
    {
        if (m_profilePanel == null)
            return;

        m_profilePanel.SetActive(false);
    }

    public void Open_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_inventoryPanel.SetActive(true);
    }

    public void Close_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_inventoryPanel.SetActive(false);
    }

    public void Open_InventoryPopup()
    {
        if (m_inventoryPopupPanel == null || Inventory == null || Inventory.Use_ItemBool() == false)
            return;

        m_inventoryPopupPanel.SetActive(true);
    }

    public void Close_InventoryPopup()
    {
        if (m_inventoryPopupPanel == null)
            return;

        m_inventoryPopupPanel.SetActive(false);
    }

    public void Open_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        m_encyclopediaPanel.SetActive(true);
    }

    public void Close_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        m_encyclopediaPanel.SetActive(false);
    }

    public void Open_Settings()
    {
        if (m_settingPanel == null)
            return;

        m_settingPanel.SetActive(true);
    }

    public void Close_Settings()
    {
        if (m_settingPanel == null)
            return;

        m_settingPanel.SetActive(false);
    }

    public void Exit_Game()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
