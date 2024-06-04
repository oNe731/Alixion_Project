using UnityEngine;

public class BirdLauncher : MonoBehaviour
{
    public GameObject birdPrefab; // �� ĳ���� ������
    public GameObject slingshotPrefab; // ���� ������

    private GameObject bird; // ���� �߻�� �� ĳ����
    private GameObject slingshot; // ���� ������Ʈ

    private Vector2 startTouchPosition; // ��ġ ���� ��ġ
    private Vector2 currentTouchPosition; // ���� ��ġ ��ġ

    void Start()
    {
        // ���� ���� �� ��Ȱ��ȭ
        slingshot = Instantiate(slingshotPrefab, Vector3.zero, Quaternion.identity);
        slingshot.SetActive(false);
    }

    void Update()
    {
        // ��ġ �Է� ó�� (���� �ڵ� ����)

        // ���콺 �巡�� ó��
        if (Input.GetMouseButtonDown(0) && Input.touchCount == 0) // ���콺 ���� ��ư ������ ��ġ�� ���� ���
        {
            startTouchPosition = Input.mousePosition; // ���콺 ��ġ�� ���� ��ġ�� ����
        }

        if (Input.GetMouseButton(0) && Input.touchCount == 0) // ���콺 ���� ��ư ������ ��ġ�� ���� ���
        {
            currentTouchPosition = Input.mousePosition; // ���콺 ��ġ�� ���� ��ġ�� ����

            // ���� ȸ�� ���� (���� �ڵ� ����)
            UpdateSlingshotRotation();
        }

        if (Input.GetMouseButtonUp(0) && Input.touchCount == 0) // ���콺 ���� ��ư �°� ��ġ�� ���� ���
        {
            LaunchBird(); // �� �߻�
        }
    }


    void UpdateSlingshotRotation()
    {
        // ��ġ ���⿡ ���� ���� ȸ�� ����
        Vector2 touchDirection = currentTouchPosition - startTouchPosition;
        float angle = Mathf.Atan2(touchDirection.y, touchDirection.x) * 180 / Mathf.PI;
        slingshot.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void LaunchBird()
    {
        // �� ĳ���� ���� �� �߻� (���� �ڵ� ����)

        // �巡�� �Ÿ� ���
        Vector2 dragDistance = currentTouchPosition - startTouchPosition;
        float dragMagnitude = dragDistance.magnitude;

        // �߻� �� ����
        float launchForceMultiplier = Mathf.Clamp(dragMagnitude / 100, 0.5f, 2.0f); // �巡�� �Ÿ��� ���� �� ����
        Vector2 launchForce = dragDistance.normalized * launchForceMultiplier * 100;

        // �� ĳ���Ϳ� �� ���ϱ�
        Rigidbody2D birdRigidbody = bird.GetComponent<Rigidbody2D>();
        birdRigidbody.AddForce(launchForce);
    }
}


