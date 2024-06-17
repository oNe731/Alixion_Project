namespace Zen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TouchScript : MonoBehaviour
    {
        [SerializeField] private int m_scoreValue = 1;
        private ZenManager m_zenManager;

        private AudioSource m_audioSource;
        private bool m_destory = false;

        private void Start()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_zenManager = FindObjectOfType<ZenManager>();
        }

        private void Update()
        {
            if (m_destory == true && m_audioSource.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }

        private void OnMouseDown()
        {
            if (m_zenManager == null || GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
                return;

            m_destory = true;
            GetComponent<AudioSource>().Play();
            m_zenManager.Add_Score(m_scoreValue);

        }

    }
}
