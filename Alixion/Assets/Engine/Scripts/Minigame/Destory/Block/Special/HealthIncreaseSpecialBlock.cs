using UnityEngine;
using System.Collections.Generic;

public class HealthIncreaseSpecialBlock : SpecialBlock
{
    protected override void Mistake_OnTouch() // ����� ���� ������ ������ 5�� ���� ü���� 2�� ����
    {
        RuinManager.Instance.Mistake_HealthIncreaseSpecialBlock();
    }
}
