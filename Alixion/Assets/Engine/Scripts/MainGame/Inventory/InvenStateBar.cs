using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenStateBar : MonoBehaviour
{
    private ALIENTYPE m_currentAlienType = ALIENTYPE.AT_END;
    private int m_currentLevel = 0;

    private Slider m_inventorySlider;
    private Image m_inventorySliderFill;

    private void Start()
    {
        m_inventorySlider = GetComponent<Slider>();
        m_inventorySliderFill = transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        m_inventorySlider.value = GameManager.Instance.SumPoint;
        if (m_currentAlienType != GameManager.Instance.CurrentAlienType || m_currentLevel != GameManager.Instance.CurrentLevel)
        {
            m_currentAlienType = GameManager.Instance.CurrentAlienType;
            m_currentLevel = GameManager.Instance.CurrentLevel;
            switch (m_currentAlienType)
            {
                case ALIENTYPE.AT_BASIC:
                    m_inventorySliderFill.color = new Color(0.2117266f, 0.8566037f, 0.2186982f, 1f);
                    break;

                case ALIENTYPE.AT_RUIN:
                    m_inventorySliderFill.color = new Color(0.6170522f, 1f, 0.5113207f, 1f);
                    break;

                case ALIENTYPE.AT_ZEN:
                    m_inventorySliderFill.color = new Color(0.5490565f, 1f, 0.9637581f, 1f);
                    break;

                case ALIENTYPE.AT_FRAUD:
                    m_inventorySliderFill.color = new Color(1f, 0.9019562f, 0.4660377f, 1f);
                    break;

                case ALIENTYPE.AT_SECLUSION:
                    m_inventorySliderFill.color = new Color(0.8045892f, 0.5792453f, 1f, 1f);
                    break;

                case ALIENTYPE.AT_MADNESS:
                    m_inventorySliderFill.color = new Color(0.3714971f, 0.5245282f, 3f, 1f);
                    break;


                case ALIENTYPE.AT_ZEN_RUIN:
                    m_inventorySliderFill.color = new Color(0.6091492f, 0.909434f, 0.7460111f, 1f);
                    break;

                case ALIENTYPE.AT_ZEN_FRAUD:
                    m_inventorySliderFill.color = new Color(0.5255607f, 0.6981132f, 0.6039937f, 1f);
                    break;

                case ALIENTYPE.AT_ZEN_SECLUSION:
                    m_inventorySliderFill.color = new Color(0.7999165f, 0.692581f, 0.9018868f, 1f);
                    break;

                case ALIENTYPE.AT_ZEN_MADNESS:
                    m_inventorySliderFill.color = new Color(0.537451f, 0.5849056f, 0.5559036f, 1f);
                    break;


                case ALIENTYPE.AT_RUIN_FRAUD:
                    m_inventorySliderFill.color = new Color(0.1200925f, 0.381132f, 0.228859f, 1f);
                    break;

                case ALIENTYPE.AT_RUIN_SECLUSION:
                    m_inventorySliderFill.color = new Color(0.2908165f, 0.274069f, 0.3433962f, 1f);
                    break;

                case ALIENTYPE.AT_RUIN_MADNESS:
                    m_inventorySliderFill.color = new Color(0.2694981f, 0.3509434f, 0.3085972f, 1f);
                    break;


                case ALIENTYPE.AT_FRAUD_SECLUSION:
                    m_inventorySliderFill.color = new Color(0.2837099f, 0.2705882f, 0.3490196f, 1f);
                    break;

                case ALIENTYPE.AT_FRAUD_MADNESS:
                    m_inventorySliderFill.color = new Color(0.22538270f, 0.2452830f, 0.22920990f, 1f);
                    break;


                case ALIENTYPE.AT_SECLUSION_MADNESS:
                    m_inventorySliderFill.color = new Color(0.2618508f, 0.1825631f, 0.3735848f, 1f);
                    break;
            }
        }
    }
}
