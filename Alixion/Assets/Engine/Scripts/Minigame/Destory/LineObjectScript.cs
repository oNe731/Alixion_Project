using UnityEngine;

public class StackDetection : MonoBehaviour
{
    // 레이캐스트 거리
    public float raycastDistance = 5f;

    void Update()
    {
        // 스택이 위에 존재하는지 감지합니다.
        DetectStackAbove();

        // 스택이 존재하지 않는지 감지합니다.
        DetectNoStackAbove();
    }

    // 스택이 위에 존재하는지 감지하는 함수
    void DetectStackAbove()
    {
        // 현재 오브젝트의 위치를 기준으로 아래 방향으로 레이캐스트를 발사합니다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance);

        // 레이캐스트가 충돌한 경우
        if (hit.collider != null)
        {
            // 충돌한 오브젝트가 "Stack" 태그를 가지고 있는지 확인합니다.
            if (hit.collider.gameObject.CompareTag("Stack"))
            {
                Debug.Log("위에 스택이 존재합니다.");
                // 여기에 필요한 동작을 추가합니다. 예를 들어 스택이 존재할 때의 처리를 수행할 수 있습니다.
            }
        }
    }

    // 스택이 존재하지 않는지 감지하는 함수
    void DetectNoStackAbove()
    {
        // 현재 오브젝트의 위치를 기준으로 아래 방향으로 레이캐스트를 발사합니다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance);

        // 레이캐스트가 충돌하지 않은 경우
        if (hit.collider == null)
        {
            Debug.Log("위에 스택이 존재하지 않습니다.");
            // 여기에 필요한 동작을 추가합니다. 예를 들어 스택이 존재하지 않을 때의 처리를 수행할 수 있습니다.
        }
    }
}
