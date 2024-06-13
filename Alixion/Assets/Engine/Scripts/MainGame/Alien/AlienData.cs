using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ALIENTYPE // 15°³
{
    AT_BASIC, 
    
    AT_RUIN, AT_ZEN, AT_FRAUD, AT_SECLUSION, AT_MADNESS,

    AT_ZEN_RUIN, AT_ZEN_FRAUD, AT_ZEN_SECLUSION, AT_ZEN_MADNESS,
    AT_RUIN_FRAUD, AT_RUIN_SECLUSION, AT_RUIN_MADNESS,
    AT_FRAUD_SECLUSION, AT_FRAUD_MADNESS,
    AT_SECLUSION_MADNESS,

    AT_END
};

public class ProfileData
{
    private string m_like;
    private string m_hate;
    private string m_state;

    public string Like => m_like;
    public string Hate => m_hate;
    public string State => m_state;

    public ProfileData(string like, string hate, string state)
    {
        m_like = like;
        m_hate = hate;
        m_state = state;
    }
}

public class AlienData
{
    private ALIENTYPE m_type;
    private string[] m_animatrNames;

    private string m_endingName;
    private string m_endingDialog;
    private string m_endingCardIamge;

    private ProfileData m_profileData;
    private List<List<string>> m_dialogs;

    public ALIENTYPE Type => m_type;
    public string[] AnimatrNames => m_animatrNames;

    public string EndingName => m_endingName;
    public string EndingDialog => m_endingDialog;
    public string EndingCardIamge => m_endingCardIamge;

    public ProfileData ProfileInfo => m_profileData;
    public List<List<string>> Dialogs => m_dialogs;


    public AlienData(
        ALIENTYPE type, 
        string animatrNames1, string animatrNames2, string animatrNames3, 
        string endingName, string endingDialog, string endingCardIamge, 
        ProfileData profileData, List<List<string>> dialogs)
    {
        m_type = type;
        m_animatrNames = new string[3];
        m_animatrNames[0] = animatrNames1;
        m_animatrNames[1] = animatrNames2;
        m_animatrNames[2] = animatrNames3;

        m_endingName      = endingName;
        m_endingDialog    = endingDialog;
        m_endingCardIamge = endingCardIamge;

        m_profileData = profileData;

        m_dialogs = new List<List<string>>();
        for (int i = 0; i < dialogs.Count; ++i)
        {
            m_dialogs.Add(new List<string>());
            for (int j = 0; j < dialogs[i].Count; ++j)
            {
                m_dialogs[i].Add(dialogs[i][j]);
            }
        }
    }
}
