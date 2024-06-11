namespace Zen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TouchScript : MonoBehaviour
    {
        [SerializeField] private int m_scoreValue = 1;
        private ZenManager m_zenManager;

        private void Start()
        {
            m_zenManager = FindObjectOfType<ZenManager>();
        }

        private void OnMouseDown()
        {
            if (m_zenManager == null || GameManager.Instance.IsMiniGame == false)
                return;

            m_zenManager.Add_Score(m_scoreValue);
            Destroy(gameObject);
        }

    }
}
