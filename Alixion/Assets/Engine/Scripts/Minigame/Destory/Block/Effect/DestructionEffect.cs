using System.Collections;
using UnityEngine;

public class DestructionEffect : MonoBehaviour
{
    [SerializeField] private float m_destructionTime = 0.2f; // �ı� ����Ʈ ���� �ð�

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(m_destructionTime);
        Destroy(gameObject);
    }
}
