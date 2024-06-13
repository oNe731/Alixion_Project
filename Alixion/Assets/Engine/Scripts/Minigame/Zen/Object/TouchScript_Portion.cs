namespace Zen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TouchScript_Portion : MonoBehaviour
    {
        private float m_timeValue = -5f;
        private ZenManager m_zenManager;

        private void Start()
        {
            m_zenManager = FindObjectOfType<ZenManager>();
        }

        private void OnMouseDown()
        {
            if (m_zenManager == null || GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
                return;

            m_zenManager.Add_TimeCount(m_timeValue);
            Destroy(gameObject);
        }
    }
}

