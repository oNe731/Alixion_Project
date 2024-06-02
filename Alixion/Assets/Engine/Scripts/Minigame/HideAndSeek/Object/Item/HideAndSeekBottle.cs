using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekBottle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹 �� ���� ���� �Ǵ� ����Ű ȥ��
        if (other.gameObject.name == "Alien")
        {
            HideAndSeekManager.Instance.Reversal = true;

            GameObject effect = other.gameObject.transform.GetChild(1).gameObject;
            effect.SetActive(true);
            effect.GetComponent<ParticleSystem>().Play();

            Destroy(gameObject);
        }
    }
}
