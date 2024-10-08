namespace Madness
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MadnessManager m_manager;

        [SerializeField] private Button       m_interactButton;
        [SerializeField] private GameObject   m_blakchole;
        [SerializeField] private Image[] m_heart;
        [SerializeField] private Sprite[] m_heartImage;
        [SerializeField] private EnemyController[] m_enemy;

        private float m_interactionRange = 2f; // 근접 상호작용 범위
        private float m_fallY = -10f;
        private float m_TPP = 10f;
        private float m_minX = -12.75f; // 이동 가능한 최소 x좌표
        private float m_maxX = 12.75f;  // 이동 가능한 최대 x좌표
        private int   m_stealscore = 0;

        private int m_maxHealth = 3;
        private int m_currentHealth = 0;

        public int Stealscore
        {
            get => m_stealscore;
            set => m_stealscore = value;
        }

        void Start()
        {
            m_currentHealth = m_maxHealth;

            if (m_interactButton != null)
                m_interactButton.onClick.AddListener(Steal_Item);
        }

        void Update()
        {
            if (transform.position.y < m_fallY)
                transform.position = new Vector3(transform.position.x, m_TPP, transform.position.z);

            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, m_minX, m_maxX);
            transform.position = position;
        }

        public void TakeDamage(int damage)
        {
            m_currentHealth -= damage;
            if (m_currentHealth < 0)
                m_currentHealth = 0;

            m_heart[m_currentHealth].sprite = m_heartImage[0];
            if (m_currentHealth <= 0)
                m_manager.Over_Game();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_stealscore > 0 && collision.gameObject.CompareTag("Blackhole"))
            {
                m_blakchole.GetComponent<AudioSource>().Play();

                transform.GetChild(0).gameObject.SetActive(false);
                m_manager.Score += m_stealscore * 5;
                m_stealscore = 0;

                for (int i = 0; i < m_enemy.Length; ++i)
                    m_enemy[i].Stop_Chase();
            }
        }

        public void Steal_Item()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_interactionRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        enemy.Steal_Item();
                        break;
                    }
                }
            }
        }
    }
}
