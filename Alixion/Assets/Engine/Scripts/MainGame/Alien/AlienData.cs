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
public class AlienData
{
    private ALIENTYPE m_type;
    private string[] m_animatrNames;

    public ALIENTYPE Type => m_type;
    public string[] AnimatrNames => m_animatrNames;

    public AlienData(ALIENTYPE type, string animatrNames1, string animatrNames2, string animatrNames3)
    {
        m_type = type;
        m_animatrNames = new string[3];
        m_animatrNames[0] = animatrNames1;
        m_animatrNames[1] = animatrNames2;
        m_animatrNames[2] = animatrNames3;
    }
}
