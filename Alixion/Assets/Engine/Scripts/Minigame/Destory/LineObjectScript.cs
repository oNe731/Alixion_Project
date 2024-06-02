using UnityEngine;

public class StackDetection : MonoBehaviour
{
    // ����ĳ��Ʈ �Ÿ�
    public float raycastDistance = 5f;

    void Update()
    {
        // ������ ���� �����ϴ��� �����մϴ�.
        DetectStackAbove();

        // ������ �������� �ʴ��� �����մϴ�.
        DetectNoStackAbove();
    }

    // ������ ���� �����ϴ��� �����ϴ� �Լ�
    void DetectStackAbove()
    {
        // ���� ������Ʈ�� ��ġ�� �������� �Ʒ� �������� ����ĳ��Ʈ�� �߻��մϴ�.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance);

        // ����ĳ��Ʈ�� �浹�� ���
        if (hit.collider != null)
        {
            // �浹�� ������Ʈ�� "Stack" �±׸� ������ �ִ��� Ȯ���մϴ�.
            if (hit.collider.gameObject.CompareTag("Stack"))
            {
                Debug.Log("���� ������ �����մϴ�.");
                // ���⿡ �ʿ��� ������ �߰��մϴ�. ���� ��� ������ ������ ���� ó���� ������ �� �ֽ��ϴ�.
            }
        }
    }

    // ������ �������� �ʴ��� �����ϴ� �Լ�
    void DetectNoStackAbove()
    {
        // ���� ������Ʈ�� ��ġ�� �������� �Ʒ� �������� ����ĳ��Ʈ�� �߻��մϴ�.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance);

        // ����ĳ��Ʈ�� �浹���� ���� ���
        if (hit.collider == null)
        {
            Debug.Log("���� ������ �������� �ʽ��ϴ�.");
            // ���⿡ �ʿ��� ������ �߰��մϴ�. ���� ��� ������ �������� ���� ���� ó���� ������ �� �ֽ��ϴ�.
        }
    }
}
