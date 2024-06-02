using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ���� ��� (���� ������Ʈ)
    public float smoothSpeed = 0.125f; // ī�޶� �̵��� �ε巯�� �ӵ�
    public Vector3 offset; // Ÿ�ٰ� ī�޶� ���� �Ÿ� ������

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
