using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour
{
    public enum FinishType { FT_CLEAR, FT_FAIL, FT_END };

    public void Finish_Game(FinishType finishType)
    {
        Camera.main.gameObject.GetComponent<AudioSource>().Stop();
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        if (finishType == FinishType.FT_CLEAR)
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Clear");
        else if(finishType == FinishType.FT_FAIL)
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Minigame/Common/UI/FontUI/Fail");
    }

    public void Create_Item(int Count, string Name1 = "", string Name2 = "", string Name3 = "")
    {
        if (Count > 0)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            GameObject UIitem1 = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + Name1), transform);
            UIitem1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-153f, -45.5f);
            GameManager.Instance.Inventory.Add_Item(UIitem1.GetComponent<ItemData>().objectName);

            if (Count > 1)
            {
                transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                GameObject UIitem2 = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + Name2), transform);
                UIitem2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.6f, -45.5f);
                GameManager.Instance.Inventory.Add_Item(UIitem2.GetComponent<ItemData>().objectName);

                if (Count > 2)
                {
                    transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                    GameObject UIitem3 = Instantiate(Resources.Load<GameObject>("Prefabs/MainGame/Inventory/Item/" + Name3), transform);
                    UIitem3.GetComponent<RectTransform>().anchoredPosition = new Vector2(153f, -45.5f);
                    GameManager.Instance.Inventory.Add_Item(UIitem3.GetComponent<ItemData>().objectName);
                }
            }
        }
    }

    public void Button_Home()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        GameManager.Instance.Go_Home();
    }

    public void Button_Retry()
    {
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sonds/Effect/MainGame/UI_Click");
        GetComponent<AudioSource>().Play();
        GameManager.Instance.Retry_Scene();
    }
}
