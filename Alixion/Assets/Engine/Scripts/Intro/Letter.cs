using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public void Click_Button()
    {
        Destroy(gameObject);
        IntroManager.Instance.Start_Letter();
    }
}
