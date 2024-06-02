using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (폴링 오브젝트)
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러운 속도
    public Vector3 offset; // 타겟과 카메라 간의 거리 오프셋

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
