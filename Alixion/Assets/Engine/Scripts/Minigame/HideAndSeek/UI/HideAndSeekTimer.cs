using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAndSeekTimer : MonoBehaviour
{
    private GameObject m_Owner;
    public GameObject Owner
    {
        set { m_Owner = value; }
    }

    private HideAndSeekSign m_owner;
    private Image m_image;

    private void Start()
    {
        m_owner = m_Owner.GetComponent<HideAndSeekSign>();
        m_image = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsMiniGame == false)
            Destroy(gameObject);

        transform.position = m_Owner.transform.position + new Vector3(-0.1f, 0.28f, 0f);
        m_image.fillAmount = m_owner.TimeInfo;
    }
}
