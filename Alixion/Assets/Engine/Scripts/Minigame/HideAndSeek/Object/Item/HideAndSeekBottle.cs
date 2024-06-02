using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekBottle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 방향 감각 또는 조작키 혼동
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
