using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodPanel : MonoBehaviour
{
    [SerializeField] private GameObject m_startPanel;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                gameObject.SetActive(false);

                if (GameManager.Instance.IsMiniGame == false)
                {
                    if(m_startPanel.activeSelf == false)
                    {
                        m_startPanel.SetActive(true);
                        m_startPanel.GetComponent<StartPanel>().Start_Ready();
                    }
                }
                else
                    GameManager.Instance.False_Pause();
            }
        }
    }
}
