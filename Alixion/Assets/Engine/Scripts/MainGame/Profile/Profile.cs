using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private Image m_bigImage;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_likeText;
    [SerializeField] private TMP_Text m_hateText;
    [SerializeField] private TMP_Text m_stateText;

    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_END;
    private int m_currentLevel = 0;
    private string m_currentName = "";

    private void Start()
    {
        m_currentAlienType = GameManager.Instance.CurrentAlienType;
        m_currentLevel = GameManager.Instance.CurrentLevel;
        Update_Profile();
    }

    private void Update()
    {
        if (m_currentLevel != GameManager.Instance.CurrentLevel)
        {
            m_currentAlienType = GameManager.Instance.CurrentAlienType;
            m_currentLevel = GameManager.Instance.CurrentLevel;
            Update_Profile();
        }

        if(m_currentName != GameManager.Instance.PlayerName)
        {
            m_currentName = GameManager.Instance.PlayerName;
            m_nameText.text = m_currentName;
        }
    }

    private void Update_Profile()
    {
        AlienData alienData = GameManager.Instance.Get_AlienDate(m_currentAlienType);

        Sprite[] sprite = null;
        string basicPath = "Sprites/MainGame/Alien/";
        switch (alienData.Type)
        {
            case ALIENTYPE.AT_BASIC:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Basic/Basic");
                break;

            case ALIENTYPE.AT_RUIN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Ruin/Ruin_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_ZEN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Zen/Zen_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Fraud/Fraud_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Seclusion/Seclusion_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "UnMixedType/Madness/Madness_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_ZEN_RUIN:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Ruin/Zen+Ruin_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_ZEN_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Fraud/Zen+Fraud_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_ZEN_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Seclusion/Zen+Seclusion_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_ZEN_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Zen+Madness/Zen+Madness_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_RUIN_FRAUD:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Fraud/Ruin+Fraud_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_RUIN_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Seclusion/Ruin+Seclusion_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_RUIN_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Ruin+Madness/Ruin+Madness_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_FRAUD_SECLUSION:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Fraud+Seclusion/Fraud+Seclusion_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_FRAUD_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Fraud+Madness/Fraud+Madness_" + m_currentLevel.ToString());
                break;

            case ALIENTYPE.AT_SECLUSION_MADNESS:
                sprite = Resources.LoadAll<Sprite>(basicPath + "MixedType/Seclusion+Madness/Seclusion+Madness_" + m_currentLevel.ToString());
                break;
        }

        if (m_bigImage != null)
            m_bigImage.sprite= sprite[0];

        m_likeText.text  = alienData.ProfileInfo.Like;
        m_hateText.text  = alienData.ProfileInfo.Hate;
        m_stateText.text = alienData.ProfileInfo.State;
    }
}
