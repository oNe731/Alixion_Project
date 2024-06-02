using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekObject : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if(gameObject.transform.position.x <= -49.64f)
        {
            gameObject.transform.position = new Vector3(24.73f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x >= 49.64f)
        {
            gameObject.transform.position = new Vector3(-24.73f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
