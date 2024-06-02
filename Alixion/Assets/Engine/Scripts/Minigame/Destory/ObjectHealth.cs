using UnityEngine;
using UnityEngine.Events;

public class ObjectHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // �� ������Ʈ ����

            // �Ʒ� Stack �±׸� ���� ������Ʈ���� ��ũ��Ʈ ����
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Stack"))
                {
                    collider.gameObject.GetComponent<ObjectHealth>().TakeDamage(damage);
                    break;
                }
            }
        }
    }
}
