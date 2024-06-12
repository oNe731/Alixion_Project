using System.Collections;
using UnityEngine;

public class TouchDisableSpecialBlock : SpecialBlock
{
    private Coroutine m_coroutine = null;

    protected override void Mistake_OnTouch() // 터치 입력을 5초간 정지
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(Freeze_Input(3f));
    }

    public IEnumerator Freeze_Input(float seconds)
    {
        bool PanelActive = RuinManager.Instance.Player.transform.GetChild(0).gameObject.activeSelf;
        if(PanelActive == true)
            RuinManager.Instance.Player.transform.GetChild(0).gameObject.SetActive(false);

        RuinManager.Instance.Player.transform.GetChild(1).gameObject.SetActive(true);
        RuinManager.Instance.CanRemoveBlock = false;

        yield return new WaitForSeconds(seconds);

        RuinManager.Instance.Player.transform.GetChild(1).gameObject.SetActive(false);
        RuinManager.Instance.CanRemoveBlock = true;

        if (PanelActive == true)
            RuinManager.Instance.Player.transform.GetChild(0).gameObject.SetActive(true);
    }
}
