using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekMethod : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                gameObject.SetActive(false);
                HideAndSeekManager.Instance.False_Pause();//HideAndSeekManager.Instance.Pause = false;
            }
        }
    }
}
