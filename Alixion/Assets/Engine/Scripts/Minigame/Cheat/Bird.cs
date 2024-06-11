namespace Fraud
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Bird : MonoBehaviour
    {
        public enum TYPE { TYPE_BIG, TYPE_SMALL, TYPE_END }

        public TYPE type = TYPE.TYPE_END;
        private FraudManager m_manager;

        private void Start()
        {
            m_manager = FindObjectOfType<FraudManager>();
        }

        private void Update()
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            if (type == TYPE.TYPE_SMALL && collision.gameObject.name == "SmallBallun(Clone)" ||
               type == TYPE.TYPE_BIG && collision.gameObject.name == "BigBallun(Clone)")
            {
                m_manager.Play_Sound("Sonds/Effect/MiniGame/CheDes_Game/ballun");

                Destroy(gameObject);
                Destroy(collision.gameObject);

                if (m_manager != null)
                    m_manager.AddScore(1);
            }
            else if (type == TYPE.TYPE_SMALL && collision.gameObject.name == "BigBallun(Clone)" ||
                type == TYPE.TYPE_BIG && collision.gameObject.name == "SmallBallun(Clone)")
            {
                Destroy(gameObject);
            }

        }
    }
}

