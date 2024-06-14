using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float m_smoothSpeed = 0.125f; // 카메라 이동의 부드러운 속도
    public Vector3 m_offset = new Vector3(2.5f, 0f, 0f); // 타겟과 카메라 간의 거리 오프셋

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
