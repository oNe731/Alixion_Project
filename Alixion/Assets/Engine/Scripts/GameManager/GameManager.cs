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

    private bool m_activePanel = false;
    private int m_sumPoint = 0;
    private int[] m_alienPoint;
    private Inventory m_inventory;
    private Encyclopedia m_encyclopedia;
    private List<AlienData> alienDatas;
    private bool m_tutorial = false;

    private bool m_isMiniGame = false;
    private bool m_pause = false;

    private string m_name;

    public ALIENTYPE CurrentAlienType => m_currentAlienType;
    public int CurrentLevel => m_currentLevel;
    public bool ActivePanel
    {
        get => m_activePanel;
        set => m_activePanel = value;
    }
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
    public string PlayerName
    {
        get => m_name;
        set => m_name = value;
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

            List<List<string>> dialogs = new List<List<string>>();
            for (int i = 0; i < 3; ++i)
                dialogs.Add(new List<string>());
            dialogs[0].Add(". . .");
            dialogs[0].Add(". . . .");
            dialogs[0].Add(". . . . .");
            dialogs[1].Add(". . .");
            dialogs[1].Add(". . . .");
            dialogs[1].Add(". . . . .");
            dialogs[2].Add(". . .");
            dialogs[2].Add(". . . .");
            dialogs[2].Add(". . . . .");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_BASIC, "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic", "UnMixedType/Basic/AC_Basic",
                "", "", "", 
                new ProfileData("?", "?", "?"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("아침에 쉬고 있는 사람을\n때리고 왔어.");
            dialogs[0].Add("쓰레기 줍는 사람이 기분 나빠서\n때리고 왔어.");
            dialogs[0].Add("아~ 아~\n세상 멸망해주지 않으려나?");
            dialogs[1].Add("쓰레기통을 다 부숴버렸어!\n하 하 하");
            dialogs[1].Add("힘을 모으고 있어.\n더 커다란 것을 파.괴. 해버릴 거야.");
            dialogs[1].Add("오늘 아침은 다 부수기에 딱 좋은 날이네!");
            dialogs[2].Add("내 주먹으로 건물을 때렸더니\n부서지더라 너무 즐거워!!");
            dialogs[2].Add("사람들이 울고불고 난리칠 때\n나는 그것을 더욱 돋구고 싶었어!");
            dialogs[2].Add("세상을 멸망 시키는 것이\n내 사명 아닐까?");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN, 
                "UnMixedType/Ruin/AC_Ruin1", "UnMixedType/Ruin/AC_Ruin2", "UnMixedType/Ruin/AC_Ruin3",
                "파괴", 
                "약 50퍼센트...즉 내 힘의 절반 정도만 내면 널 우주의 먼지로 만들어 버릴 수 있어...\n< 지옥 그 이상의 공포를 보여드리죠. >",
                "Ruin + Ruin Ending",
                new ProfileData("파괴, 파멸, 죽음, 전쟁.", "희망, 생명.", "전부 부숴버리고 싶다!"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("아침햇살이 너무 밝아서\n기분이 너~무 좋아!");
            dialogs[0].Add("나는 세상에서\n가장 선한 존재가 될 거야!");
            dialogs[0].Add("멋진 하루를\n시작할 준비가 되었어!");
            dialogs[1].Add("오늘은 밖에 나가서\n쓰레기를 줍고 왔어.\n잘했지?");
            dialogs[1].Add("교황님한테 고해성사를 하고 왔어...\n너무 멋진 분이시더라.");
            dialogs[1].Add("나는 인간들에게\n도움을 주고 싶은 마음이 가득해!");
            dialogs[2].Add("오늘 성당에서 미사를 보고 왔어.\n열심히 신에게 기도를 드렸어.\n온 세상이 평화롭기를...");
            dialogs[2].Add("교황님과 많이 친해졌어.\n내가 분명 다른 존재임에도 불구하고\n평등하게 대해주셨어.");
            dialogs[2].Add("오늘 도움이 필요한 사람을\n여럿 도와주고 왔어,\n너무 행복한 하루야.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN, 
                "UnMixedType/Zen/AC_Zen1", "UnMixedType/Zen/AC_Zen2", "UnMixedType/Zen/AC_Zen3",
                "선", 
                "새로운 성인의 탄생이다!!\n< 그는 신이야... >",
                "Zen + Zen Ending",
                new ProfileData("착한 짓!, 나눔, 의리, 행복.", "나쁜 짓..., 못된 짓, 거짓말.", "상쾌하고 항상 웃고 싶은 기분~"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("오늘 쓰레기통을 뒤져서 돈을 주웠어!!!\n하하하.");
            dialogs[0].Add("오늘은 돈 벌기 딱 좋은 날씨네.");
            dialogs[0].Add("하하하!\n돈이 최고야 하하하!");
            dialogs[1].Add("무슨 소리야 친구!\n돈보다 좋은 것은 없어!");
            dialogs[1].Add("돈으로 해결할 수 없는 일을 없어!");
            dialogs[1].Add("돈 준다는데, 뭘 못하겠어!");
            dialogs[2].Add("어디 보자, 행복을 위한 조건이라고?\n그럼 돈이지, 뭐.");
            dialogs[2].Add("지옥의 판결도 가진 돈\n나름대로야.");
            dialogs[2].Add("나는 돈이 좋다.\n왜냐하면 돈은 모든 것을\n대신할 수 있기 때문이다.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD, 
                "UnMixedType/Fraud/AC_Fraud1", "UnMixedType/Fraud/AC_Fraud2", "UnMixedType/Fraud/AC_Fraud3",
                "사기",
                "내가 팔지 못하는 것은 없다. 돈은 항상 옳다!\n< 그는 희대의 사기꾼이야. >",
                "Fraud + Fraud Ending",
                new ProfileData("돈, 공짜, 사기, 기만.", "사치, 믿음, 선의.", "돈이 정말 좋아~!!"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("...사람들의 시선이 두려워.");
            dialogs[0].Add("사람들의 시선이 익숙해지지 않아...");
            dialogs[0].Add("이것을 활용할.. 수 있을까...?");
            dialogs[1].Add("사람들의 시선에 민감해서\n몰래 다니기 쉬워졌어!");
            dialogs[1].Add("이런 식이면 좀 멋진 짓을\n할 수 있을지 몰라.");
            dialogs[1].Add("오늘 사람을 좀 구해줬어.\n내 정체는 모르겠지만...");
            dialogs[2].Add("은신하고 구원하고\n자유를 전파한다.");
            dialogs[2].Add("보이지 않는 내가\n가장 무서운 법이지.");
            dialogs[2].Add("사람들은 쥐도 새도 모른 체\n죽거나 살 것이다.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION, 
                "UnMixedType/Seclusion/AC_Seclusion1", "UnMixedType/Seclusion/AC_Seclusion2", "UnMixedType/Seclusion/AC_Seclusion3",
                "은둔", 
                "족쇄는 끊어져야 하고 모든 인간은 자유로워져야 한다.\n< 우리는 빛을 섬기며 어둠 속에서 움직인다. >",
                "Seclusion + Seclusion Ending",
                new ProfileData("질서, 그림자, 자유.", "빛, 시선, 억압.", ",,,"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("참 거지같은 날이었어.");
            dialogs[0].Add("항상... 부정적인 생각뿐인데.");
            dialogs[0].Add("계단에서 내 마음을 표현하고 싶군.");
            dialogs[1].Add("나만 미친 건가요,\n세상이 더 미쳐가는 건가요?");
            dialogs[1].Add("중요한 건 돈이 아냐,\n메세지지.\n모든 것은 불탄다!");
            dialogs[1].Add("이거 봐, 난 괴물이 아냐.\n그냥 시대를 앞선 거지.");
            dialogs[2].Add("광기란 건, 알다시피, 중력 같은 거야!\n살짝 밀어 주기만 하면 되거든!");
            dialogs[2].Add("난장판을 좀 만들어 놓고,\n정립된 질서를 뒤엎으면\n모든 게 혼돈에 빠지지.");
            dialogs[2].Add("나는 혼돈의 대리인이야.\n그리고 혼돈의 특징이 뭔지 알아?\n공평하다는 거야.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_MADNESS, 
                "UnMixedType/Madness/AC_Madness1", "UnMixedType/Madness/AC_Madness2", "UnMixedType/Madness/AC_Madness3",
                "광기", 
                "내 신념은 말이지, 죽을 만큼의 고난은 사람을... '광'하게 만든다는 거야.\n< 왜 그리 심각해? > ",
                "Madness + Madness Ending",
                new ProfileData("히히히히히히", "하하하하하", "ㅎㅎㅎㅎㅎㅎㅎㅎ"), dialogs));


            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("오늘도 멋진 하루야!\n하지만 뭔가 마음에 걸려...");
            dialogs[0].Add("세상엔 정말 멋진 게 많아!\n하지만 아쉬운 것도...");
            dialogs[0].Add("나는 아마 멋진 사람이 될 거야!");
            dialogs[1].Add("오늘 아침에 쓰레기를 줍고 왔어...\n왜 이렇게 쓰레기가 많은 걸까?");
            dialogs[1].Add("교황님과 얘기를 해보았어,\n하지만 교황님은 나랑 다르게\n모든 사람을 포용하려고 해.");
            dialogs[1].Add("힘이... 힘이 필요해...\n내 꿈을 이루기 위해서는.");
            dialogs[2].Add("세상의 균형을 맞추어야만 해.");
            dialogs[2].Add("모든 생명체가 존속하기 위한 방법은\n단 하나 뿐이야.");
            dialogs[2].Add("내 힘이 거의 차올랐어.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_RUIN, 
                "MixedType/Zen+Ruin/AC_Zen_Ruin1", "MixedType/Zen+Ruin/AC_Zen_Ruin2", "MixedType/Zen+Ruin/AC_Zen_Ruin3",
                "선 + 파괴", 
                "이곳에 있는 인간은 너무나도 더러운 종족이다..\n이렇게 많은 인간은 필요가 없어.\n< 쓰레기는 제거해야 해, 난 필연적인 존재다. >",
                "Zen + Ruin Ending",
                new ProfileData("선, 평화, 균형.", "악, 파괴.", "선을 항상 지켜야만해..."), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("오늘 사람들 앞에서 쓰레기를 줍고 왔어!\n많이 칭찬을 해주시더라고!");
            dialogs[0].Add("길에 넘어져서 엎드려져 있었는데,\n사람들이 돈을 주더라!!!\n완전 기분 좋았어!!!!");
            dialogs[0].Add("내 생각엔...\n돈이...좋은..걸까?");
            dialogs[1].Add("사람들을 많이 돕고 싶어!");
            dialogs[1].Add("오늘 길거리에서 쓰레기를 치우고,\n그걸 팔아서 돈을 벌었어!");
            dialogs[1].Add("사람들을 많이 돕고 싶지만,\n그렇기 위해선 돈이 많이 필요할 거야!");
            dialogs[2].Add("사람들을 돕기에는 너무 많아...\n규율이 필요해.");
            dialogs[2].Add("사람들이 내 말에 따르기만 한다면....\n분명 좋은 세상이 만들어질 거야.");
            dialogs[2].Add("오늘 미사를 보고 왔는데\n매우 환상적이었어!\n사람도 돕고! 돈도 걷고!");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_FRAUD, 
                "MixedType/Zen+Fraud/AC_Zen_Fraud1", "MixedType/Zen+Fraud/AC_Zen_Fraud2", "MixedType/Zen+Fraud/AC_Zen_Fraud3",
                "선 + 사기", 
                "거짓은 고한 적이 없다. 다만 모든 진실을 말하지 않아 사죄한다.\n< 나는 교황이다. 인간의 존속은 반드시 필요하다. >",
                "Zen + Fraud Ending",
                new ProfileData("위선, 선.", "단순한 선의, 악.", "앞에선 굽신굽신, 뒤에선 칼로 찌르고 싶은 기분!"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("오늘도 좋은 아침이야!\n하지만 사람들 앞에 서기 부끄러워...");
            dialogs[0].Add("사람들 몰래 쓰레기를 주웠어!\n기분이 좋았어!");
            dialogs[0].Add("우...\n사람들에게 모습을 보였어...");
            dialogs[1].Add("사람들 몰래 움직이는 것에 익숙해졌어!");
            dialogs[1].Add("몰래 성당에 잠입해서 미사를 보고 왔어!\n교황님 멋지시더라...");
            dialogs[1].Add("아침의 시작은 몰래몰래 길거리 청소하기!");
            dialogs[2].Add("교황님을 몰래 노리던 인물들을 몰래 처리했어!\n교황님은 눈치채지 못했지만...");
            dialogs[2].Add("교황님 몰래 지키는 일을 하니\n너무 뿌듯했어!");
            dialogs[2].Add("사람들을 돕는 방법에는\n여러가지가 있는 것 같아!");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_SECLUSION, 
                "MixedType/Zen+Seclusion/AC_Zen_Seclusion1", "MixedType/Zen+Seclusion/AC_Zen_Seclusion2", "MixedType/Zen+Seclusion/AC_Zen_Seclusion3",
                "선 + 은둔",
                "교황님 항상 제가 지켜드리겠습니다.\n< 당신은 내마음의 100점 ❤️ >",
                "Zen + Seclusion Ending",
                new ProfileData("몰래 착한짓 하기, 몰래 산책하며 쓰레기 줍기, 교황님!", "사람들에게 모습 들키기.", "나는 착한게 좋아! 칭찬받지 않더라도!"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("나는 쓰레기를~\n열심히 치웠어~~");
            dialogs[0].Add("히히 나는 착한 게 제일 좋아,\n하지만 악은 너무 싫어~");
            dialogs[0].Add("오늘은 어떤 착한 일을 할까~~");
            dialogs[1].Add("오늘 교황님의 말씀을 듣고 왔어.\n세상에 악인은 없다 하시더라...\n그런데 내 생각은 달라~");
            dialogs[1].Add("악인에 대한 정보를 한번 적어 볼까~~");
            dialogs[1].Add("악~~인은~~\n처단~~해야지~~");
            dialogs[2].Add("악인은 처단.\n악인은 처단.\n악인은 처단.");
            dialogs[2].Add("예수님 저는 당신의 검입니다.");
            dialogs[2].Add("저는 규율에 따라 일을 행할 것입니다.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_ZEN_MADNESS, 
                "MixedType/Zen+Madness/AC_Zen_Madness1", "MixedType/Zen+Madness/AC_Zen_Madness2", "MixedType/Zen+Madness/AC_Zen_Madness3",
                "선 + 광기",
                "이 기도를 주 예수의 이름으로 진심을 담아 드립니다.\n< 폭력을 휘둘러도 되는 대상은 이교도와 괴물 놈들뿐. >",
                "Zen + Madness Ending",
                new ProfileData("선인~~, 과업.", "악인~~, 업보.", "선을 항상 중시하고 있습니다."), dialogs));


            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("아 뭔가 부수고 싶은 욕망이 들어.\n하지만 내손으로는 싫은걸?");
            dialogs[0].Add("오늘 아침에 서로 싸우다가 화해 하는 사람을\n다시 부추겨서 싸우도록 했어!");
            dialogs[0].Add("나는 사람들 통수 치는 게 재밌어.\n헤헤.");
            dialogs[1].Add("오늘 밖에 나와서 선동을 했어!\n사람들이 내 의도 대로 잘 따라주던걸?");
            dialogs[1].Add("나는 이일이 너무 재미있어!\n근데 뭔가 아쉬워 전쟁이라도 일어나주지..");
            dialogs[1].Add("오늘 짜증나서 길거리에 있는 물건들을 부쉈어.\n약간 기분이 나아지긴 했지만 아쉬워...");
            dialogs[2].Add("오늘 사람들에게 차별을 인식시켰어.\n정말 좋아하더라니깐~");
            dialogs[2].Add("적으로 간주하기만 하면\n인간들은 정말 무섭다니까~");
            dialogs[2].Add("제노사이드...\n아, 이 시대에는 없는 말인가?");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_FRAUD, 
                "MixedType/Ruin+Fraud/AC_Ruin_Fraud1", "MixedType/Ruin+Fraud/AC_Ruin_Fraud2", "MixedType/Ruin+Fraud/AC_Ruin_Fraud3",
                "파괴 + 사기",
                "전쟁이 곧 나이니라, 전쟁을 하거라.\n미친놈은 미친놈이 잡아야지, 내손에 피를 묻히진 않는다.\n< 분열하여 지배하라. >",
                "Ruin + Fraud Ending",
                new ProfileData("돈, 싸움구경, 파멸, 몰락, 통수, 선동, 전쟁.", "화해, 가난, 의리.", "뒤에서 무언가를 꾸미고 있어!"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("사람들에게 몰래몰래 접근하고 있어!");
            dialogs[0].Add("오늘은 몰래 쓰레기통을 부수고 왔어.\n너무 재밌어!");
            dialogs[0].Add("사람들 앞에 서는 건 좀 그래...");
            dialogs[1].Add("아침마다 몰래 쓰레기통을 부수고 다니니\n쓰레기가 너무 많아졌어.");
            dialogs[1].Add("사람들 몰래 움직이는 것에 익숙해졌어!\n그러다보니 좀 더 과감해졌어!");
            dialogs[1].Add("몰래몰래 파괴시키는 것은 없을까?");
            dialogs[2].Add("폭..탄 이라고 알아?");
            dialogs[2].Add("폭발은 너무 멋지더라!!!\n뻥이요~~!");
            dialogs[2].Add("몰래몰래 설치하고 폭발시키고\n너무 재밌어!");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_SECLUSION, 
                "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion1", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion2", "MixedType/Ruin+Seclusion/AC_Ruin_Seclusion3",
                 "파괴 + 은둔",
                 "몸과 마음이 우주를 향해서 아무런 까닭 없이 번쩍하며 펼쳐내는 것.\n그것이 폭발이다.\n< 예술은 폭발이다!!! >",
                 "Ruin + Seclusion Ending",
                new ProfileData("폭발, 파괴, 어둠.", "사람, 시선, 방어.", "몰래 펑! 터트리고 싶당."), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("하하하.\n정말 정말 끔찍한 하루의 시작이야!");
            dialogs[0].Add("히히히\n왜 나는 다 때려 부수고 싶을까~~~");
            dialogs[0].Add("나는. 나는. 파괴를 했다!");
            dialogs[1].Add("히히히! 후후! 헤에~헤에!");
            dialogs[1].Add("후후후후후후\n혼란한게 최고야.");
            dialogs[1].Add("전쟁....\n전쟁이 필요해.");
            dialogs[2].Add("전쟁이 일어나면\n얼마나 즐거울까~");
            dialogs[2].Add("사람들은 아직 전쟁의 즐거움을 몰라~\n내가 기쁨을 알려줘야해!~");
            dialogs[2].Add("후후 정말 재미있는 걸?\n좀 더 크게 놀고 싶어!!!");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_RUIN_MADNESS, 
                "MixedType/Ruin+Madness/AC_Ruin_Madness1", "MixedType/Ruin+Madness/AC_Ruin_Madness2", "MixedType/Ruin+Madness/AC_Ruin_Madness3",
                "파괴 + 광기",
                "겪어보지 못한 자에게 전쟁이란 달콤한 것이다.\n< 혼란하다! 혼란해! 하하하.>",
                "Ruin + Madness Ending",
                new ProfileData("비웃음, 파괴, 살의.", "웃음, 희망, 미래.", "히히히 파.괴.조.아."), dialogs));


            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("음~ 돈이 좋아!\n하지만 뭔가 부족한걸?");
            dialogs[0].Add("사람들 앞에 서긴 좀 그래...");
            dialogs[0].Add("몰래몰래 사람들한테\n돈을 뜯어내고 싶다.");
            dialogs[1].Add("내가 하고 싶은 게 생겼어!\n하지만 머리가 엄~청 좋아야 해.");
            dialogs[1].Add("무슨 공부를 하냐고 비밀이야!");
            dialogs[1].Add("후후후 역시 머리 아플 땐\n사람들을 골려먹어야 제맛이야~");
            dialogs[2].Add("준비가 거의 다 되었어!\n정말 열심히 공부했어!");
            dialogs[2].Add("사전 작업을 열심히 해야겠어~");
            dialogs[2].Add("후훗 정말 기대가 되는걸?\n사람들이 내 '말'이 되는 것이.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_SECLUSION, 
                "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion1", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion2", "MixedType/Fraud+Seclusion/AC_Fraud_Seclusion3",
                "사기 + 은둔", 
                "뭔가를 캐내려고 하지 마라... 답하지 않을 터이니...\n< 한 누군가의 뜻대로 세계가 움직인다. >",
                "Fraud + Seclusion Ending",
                new ProfileData("그림자, 돈, 사기.", "내 뜻대로 안 되는 것, 나서는 것.", "모든 것은 내 뜻대로 되면 좋겠어~"), dialogs));

            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("나는 남들과는 달라...\n이유가 있지 않을깡?");
            dialogs[0].Add("사람들을 속여서\n쓰레기로 돈을 벌었엉!");
            dialogs[0].Add("사람들은 생각보다\n너무 잘 믿는 거 아닐깡?");
            dialogs[1].Add("오늘 교황을 만나고 왔엉?\n나도 그런것이 되고 싶당...");
            dialogs[1].Add("내가 다르게 태어난 이유는\n사람들 위에 있기 위한 게 아닐깡?");
            dialogs[1].Add("나는 신... 일 수도\n있지 않을까?");
            dialogs[2].Add("내 신자를 만들었어!\n나는 누군가의 믿음 그 자체야.");
            dialogs[2].Add("후하...\n내가 곧 법이지?");
            dialogs[2].Add("나는... 모든 사람들이\n숭배해야만 하는 존재야.");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_FRAUD_MADNESS, 
                "MixedType/Fraud+Madness/AC_Fraud_Madness1", "MixedType/Fraud+Madness/AC_Fraud_Madness2", "MixedType/Fraud+Madness/AC_Fraud_Madness3",
                "사기 + 광기", 
                "나를 믿지 않으면 세상에서 지워지도록 만들겠다.\n< 나는 신이다. >",
                "Fraud + Madness Ending",
                new ProfileData("사기 치기, 미친 짓 하기, 삥 뜯기, 본인.", "행복해하기, 사랑.", "나는 위대해."), dialogs));


            for (int i = 0; i < 3; ++i)
                dialogs[i].Clear();
            dialogs[0].Add("히히히 나는 사람들이 무서웡~");
            dialogs[0].Add("오늘 교황님을 봤어!!!! 너무 멋져!!!");
            dialogs[0].Add("교황님...");
            dialogs[1].Add("감히 내 교황님을\n넘보는 놈들이 있길래.\n혼쭐을 내줬어.");
            dialogs[1].Add("아 교황님.\n당신이 없는 세상에선\n전 살 수 없어요.");
            dialogs[1].Add("아 교황님! 아 교황님!!!");
            dialogs[2].Add("좋을 대로 하세요.\n사랑의 형태는 저마다 제각각인 거니까.\n굳이 절 이해해 달라고 하지 않았어요.");
            dialogs[2].Add("교황님에게, 교황, 교황님에, 교황님이라,\n사랑해, 사랑을, 사랑이! 사랑합니다!\n사랑받고 있는 것입니다!");
            dialogs[2].Add("교황님, 당신이, 당신이 나를, 나로 만들었어!\n잠시 잊어도 좋을, 리 없습니다...\n당신이 잊어도, 나는, 잊을 수, 없어!");
            alienDatas.Add(new AlienData(ALIENTYPE.AT_SECLUSION_MADNESS, 
                "MixedType/Seclusion+Madness/AC_Seclusion_Madness1", "MixedType/Seclusion+Madness/AC_Seclusion_Madness2", "MixedType/Seclusion+Madness/AC_Seclusion_Madness3",
                "은둔 + 광기",
                "너를 죽이려는 것들은 모조리 죽여버리면 돼!!\n< 괜찮아, 너는... 내가 지켜줄게. >",
                "Madness + Seclusion Ending",
                new ProfileData("교황님.", "교황님을 제외한 전부.", "교황님... 교황님..."), dialogs));
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
        m_sumPoint = 0;
        for (int i = 0; i < m_alienPoint.Length; ++i)
            m_sumPoint += m_alienPoint[i];

        if (m_sumPoint < 15) { m_currentLevel = 1; } // 1단계
        else if (m_sumPoint < 30) { m_currentLevel = 2; } // 2단계
        else if (m_sumPoint < 45) { m_currentLevel = 3; } // 3단계

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
        if (m_currentLevel == 3 && m_sumPoint >= 45) { StartCoroutine(Create_EndingCard(alienData)); }
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

    public AlienData Get_AlienDate(ALIENTYPE alienType)
    {
        return alienDatas[(int)alienType];
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

        m_activePanel = true;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_profilePanel.SetActive(true);
    }

    public void Close_Profile()
    {
        if (m_profilePanel == null)
            return;

        m_activePanel = false;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_profilePanel.SetActive(false);
    }

    public void Open_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_activePanel = true;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryPanel.SetActive(true);
    }

    public void Close_Inventory()
    {
        if (m_inventoryPanel == null)
            return;

        m_activePanel = false;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_inventoryPanel.SetActive(false);
        m_inventory.SelctSlot = null;
    }

    public void Open_InventoryItem(ItemData m_item)
    {
        if (m_inventoryItemPanel == null || Inventory == null)
            return;

        if(Inventory.Use_ItemBool() == false)
        {
            Close_InventoryItem();
            return;
        }

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

        m_activePanel = true;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_encyclopediaPanel.SetActive(true);
    }

    public void Close_Encyclopedia()
    {
        if (m_encyclopediaPanel == null)
            return;

        m_activePanel = false;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_encyclopediaPanel.SetActive(false);
    }

    public void Open_Settings()
    {
        if (m_settingPanel == null)
            return;

        m_activePanel = true;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_settingPanel.SetActive(true);
    }
    
    public void Close_Settings()
    {
        if (m_settingPanel == null)
            return;

        False_Pause();

        m_activePanel = false;
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        m_settingPanel.SetActive(false);
    }

    public void Exit_Game()
    {
        m_activePanel = false;
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

        m_activePanel = true;
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
        if (m_pause == false)
            return;

        StartCoroutine(Wait_Pause());
    }

    private IEnumerator Wait_Pause()
    {
        float time = 0;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_pause = false;
        Camera.main.GetComponent<AudioListener>().enabled = true;

        yield break;
    }

    public void Retry_Scene()
    {
        m_isMiniGame = false;
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 0f, false); // 해당 씬 재시작
    }

    public void Go_Home()
    {
        m_isMiniGame = false;
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => StartCoroutine(GameManager.Instance.Wait_LodeScene(ScreenOrientation.Portrait, "MainGame")), 0f, false);
    }
}
