using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab; // 버튼 프리팹

    public int maxButtons = 15; // 최대 버튼 생성 개수

    public List<GameObject> buttons = new List<GameObject>(); // 생성된 버튼 목록

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        for (int i = 0; i < maxButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, GetRandomPosition(), Quaternion.identity);
            newButton.transform.SetParent(transform); // 버튼을 관리하는 오브젝트에 자식으로 설정

            // 버튼에 고유 번호 할당 (인스펙터 창에서 설정된 값 사용)
            newButton.GetComponent<Button>().buttonNumber = i + 1; // Button 스크립트에 buttonNumber 속성 추가

            buttons.Add(newButton); // 생성된 버튼 목록에 추가

            // 버튼 위치 조정 (겹치지 않도록)
           
        }
    }

    Vector2 GetRandomPosition()
    {
        // 화면 범위 내에서 랜덤 위치 계산
        float randomX = Random.Range(-2.0f, 2.0f);
        float randomY = Random.Range(-4.0f, 4.0f);

        return new Vector2(randomX, randomY);
    }


}
