using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public bool collided;
    public bool isSmall = false;
    public bool isBig = false;


    public void Release()
    {
        PathPoints.instance.Clear();
        StartCoroutine(CreatePathPoints());
    }

    IEnumerator CreatePathPoints()
    {
        while (true)
        {
            if (collided) break;
            PathPoints.instance.CreateCurrentPathPoint(transform.position);
            yield return new WaitForSeconds(PathPoints.instance.timeInterval);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Word_Big"))
    //    {
    //        if (isSmall == true)
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //        Destroy(collision.gameObject); // 충돌한 오브젝트 파괴
    //        Destroy(gameObject); // 자신도 파괴
    //        ScoreManager.instance.IncreaseScore(); // 점수 증가
    //    }
    //    if (collision.gameObject.CompareTag("Word_Small"))
    //    {
    //        if (isBig == true)
    //        {
    //            Destroy(gameObject);
    //            return;
    //        }
    //        Destroy(collision.gameObject); // 충돌한 오브젝트 파괴
    //        Destroy(gameObject); // 자신도 파괴
    //        ScoreManager.instance.IncreaseScore(); // 점수 증가
    //    }
    //    collided = true;
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Word_Big"))
        {
            if (isSmall == true)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(collision.gameObject); // 충돌한 오브젝트 파괴
            Destroy(gameObject); // 자신도 파괴
            ScoreManager.instance.IncreaseScore(); // 점수 증가
        }
        if (collision.gameObject.CompareTag("Word_Small"))
        {
            if (isBig == true)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(collision.gameObject); // 충돌한 오브젝트 파괴
            Destroy(gameObject); // 자신도 파괴
            ScoreManager.instance.IncreaseScore(); // 점수 증가
        }
        collided = true;
    }
}
