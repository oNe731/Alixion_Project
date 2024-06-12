using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))
        {
            if (this.CompareTag("Player"))
                RuinManager.Instance.Check_GameOver("Player");
            else if (this.CompareTag("AI1"))
                RuinManager.Instance.Check_GameOver("AI1");
            else if (this.CompareTag("AI2"))
                RuinManager.Instance.Check_GameOver("AI2");
        }
    }
}
