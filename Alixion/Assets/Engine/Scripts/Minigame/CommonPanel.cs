using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_methodPanel;
    [SerializeField] private GameObject m_settingPanel;

    public void Button_MethodPanel()
    {
        GameManager.Instance.Pause = true;
        m_methodPanel.SetActive(true);

        Camera.main.GetComponent<AudioListener>().enabled = false;
    }

    public void Button_SettingPanel()
    {
        GameManager.Instance.Pause = true;
        m_settingPanel.SetActive(true);

        Camera.main.GetComponent<AudioListener>().enabled = false;
    }
}
