using UnityEngine;
using System.Collections.Generic;

public class HealthIncreaseSpecialBlock : SpecialBlock
{
    protected override void Mistake_OnTouch() // 스페셜 블럭을 제외한 최하위 5개 블럭의 체력을 2로 설정
    {
        RuinManager.Instance.Mistake_HealthIncreaseSpecialBlock();
    }
}
