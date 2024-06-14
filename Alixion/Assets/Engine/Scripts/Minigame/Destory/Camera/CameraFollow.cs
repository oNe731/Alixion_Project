using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float m_smoothSpeed = 0.125f; // ī�޶� �̵��� �ε巯�� �ӵ�
    public Vector3 m_offset = new Vector3(2.5f, 0f, 0f); // Ÿ�ٰ� ī�޶� ���� �Ÿ� ������

    private void Start()
    {
        transform.position = Vector3.Lerp(transform.position, RuinManager.Instance.Player.transform.position + m_offset, m_smoothSpeed);
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
            return;

        transform.position = Vector3.Lerp(transform.position, RuinManager.Instance.Player.transform.position + m_offset, m_smoothSpeed);
    }
}
