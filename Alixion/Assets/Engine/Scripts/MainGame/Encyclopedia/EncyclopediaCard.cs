using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaCard : MonoBehaviour
{
    [SerializeField] TMP_Text m_nameTxt;
    [SerializeField] TMP_Text m_dialogTxt;
    [SerializeField] Image m_cardImage;

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void Set_Card(AlienData alienData, bool button)
    {
        m_nameTxt.text     = alienData.EndingName;
        m_dialogTxt.text   = alienData.EndingDialog;
        m_cardImage.sprite = Resources.Load<Sprite>("Sprites/MainGame/Ending/" + alienData.EndingCardIamge);

        if(button == true)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);

            string path = "Sonds/BGM/Ending/";
            if(alienData.Type == ALIENTYPE.AT_FRAUD)
            {
                path += "Fraud_Ending";
            }
            else if (alienData.Type == ALIENTYPE.AT_MADNESS)
            {
                path += "Madness_Ending";
            }
            else if(alienData.Type == ALIENTYPE.AT_RUIN)
            {
                path += "Ruin_Ending";
            }
            else if (alienData.Type == ALIENTYPE.AT_SECLUSION)
            {
                path += "Seclusion_Ending";
            }
            else if (alienData.Type == ALIENTYPE.AT_ZEN)
            {
                path += "Zen_Ending";
            }
            else
            {
                path += "Mix_Ending";
            }

            Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(path);
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }

    public void Close_Card()
    {
        GameManager.Instance.Play_Sound("Sonds/Effect/MainGame/UI_Click");
        Destroy(gameObject);
    }

    public void Button_Start()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();

        GameManager.Instance.Reset_AlienStat();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => GameManager.Instance.Start_WaitLodeScene(ScreenOrientation.Portrait, "StartScreen"), 0f, false);
    }

    public void Button_Retry()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();

        GameManager.Instance.Reset_AlienStat();
        Main.UIManager.Instance.Start_FadeOut(0.5f, Color.black, () => GameManager.Instance.Start_WaitLodeScene(ScreenOrientation.Portrait, "MainGame"), 0f, false);
    }
}
