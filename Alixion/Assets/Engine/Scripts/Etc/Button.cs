using UnityEngine;

public class Button : MonoBehaviour
{
    // 버튼 고유 번호 (인스펙터 창에서 직접 설정)
    public int buttonNumber;

    // 버튼 클릭 이벤트 처리 (예: HandleButtonInput 함수 호출)
    void OnClick()
    {
        HandleButtonInput();
    }

    void HandleButtonInput()
    {
        // 특정 상황 발생 (예: 버튼 색 변경, 사운드 재생, 새로운 씬 로드)
        // ...
    }
}
