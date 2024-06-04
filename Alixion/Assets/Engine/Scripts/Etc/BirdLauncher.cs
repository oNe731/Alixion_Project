using UnityEngine;

public class BirdLauncher : MonoBehaviour
{
    public GameObject birdPrefab; // 새 캐릭터 프리팹
    public GameObject slingshotPrefab; // 새총 프리팹

    private GameObject bird; // 현재 발사된 새 캐릭터
    private GameObject slingshot; // 새총 오브젝트

    private Vector2 startTouchPosition; // 터치 시작 위치
    private Vector2 currentTouchPosition; // 현재 터치 위치

    void Start()
    {
        // 새총 생성 및 비활성화
        slingshot = Instantiate(slingshotPrefab, Vector3.zero, Quaternion.identity);
        slingshot.SetActive(false);
    }

    void Update()
    {
        // 터치 입력 처리 (기존 코드 유지)

        // 마우스 드래그 처리
        if (Input.GetMouseButtonDown(0) && Input.touchCount == 0) // 마우스 왼쪽 버튼 누르고 터치가 없는 경우
        {
            startTouchPosition = Input.mousePosition; // 마우스 위치를 시작 위치로 설정
        }

        if (Input.GetMouseButton(0) && Input.touchCount == 0) // 마우스 왼쪽 버튼 누르고 터치가 없는 경우
        {
            currentTouchPosition = Input.mousePosition; // 마우스 위치를 현재 위치로 설정

            // 새총 회전 조정 (기존 코드 유지)
            UpdateSlingshotRotation();
        }

        if (Input.GetMouseButtonUp(0) && Input.touchCount == 0) // 마우스 왼쪽 버튼 뗏고 터치가 없는 경우
        {
            LaunchBird(); // 새 발사
        }
    }


    void UpdateSlingshotRotation()
    {
        // 터치 방향에 따라 새총 회전 조정
        Vector2 touchDirection = currentTouchPosition - startTouchPosition;
        float angle = Mathf.Atan2(touchDirection.y, touchDirection.x) * 180 / Mathf.PI;
        slingshot.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void LaunchBird()
    {
        // 새 캐릭터 생성 및 발사 (기존 코드 유지)

        // 드래그 거리 계산
        Vector2 dragDistance = currentTouchPosition - startTouchPosition;
        float dragMagnitude = dragDistance.magnitude;

        // 발사 힘 조절
        float launchForceMultiplier = Mathf.Clamp(dragMagnitude / 100, 0.5f, 2.0f); // 드래그 거리에 따라 힘 조절
        Vector2 launchForce = dragDistance.normalized * launchForceMultiplier * 100;

        // 새 캐릭터에 힘 가하기
        Rigidbody2D birdRigidbody = bird.GetComponent<Rigidbody2D>();
        birdRigidbody.AddForce(launchForce);
    }
}


