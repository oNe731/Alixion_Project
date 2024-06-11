namespace Madness
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private PlayerController m_playerControl;
        [SerializeField] private LayerMask m_groundLayer;

        private Transform   m_playerTr;
        private Animator    m_animator;
        private Rigidbody2D m_rigidbody;

        private float m_moveSpeed = 2f; // NPC �̵� �ӵ�

        private bool m_facingRight = true;
        private bool m_isChasing = false;
        private bool m_hasStolenItem = false;

        private float m_fallY = -10.0f;
        private float m_TPP = 10.0f;    // �ڷ���Ʈ ��ǥ
        private float m_minX = -12.75f; // �̵� ������ �ּ� x��ǥ
        private float m_maxX = 12.75f;  // �̵� ������ �ִ� x��ǥ

        private float m_chaseDuration = 15f; // �ѱ� �ð� (10��)
        private float m_chaseTimer = 0f;     // �ѱ� Ÿ�̸�

        private float m_collisionCooldown = 2.0f; // �����ð�
        private float m_lastCollisionTime;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_animator.StartPlayback(); // �ִϸ��̼� ����
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            m_lastCollisionTime = -m_collisionCooldown;
            m_playerTr = m_playerControl.gameObject.transform;
        }

        void Update()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            if (!m_isChasing)
                Move_OnPlatform();
            else
                Chase_Player();

            if (transform.position.y < m_fallY)
                transform.position = new Vector3(transform.position.x, m_TPP, transform.position.z);

            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, m_minX, m_maxX);
            transform.position = position;
        }

        void Move_OnPlatform()
        {
            // �¿� �̵�
            float horizontalInput = m_facingRight ? 1f : -1f;
            m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);

            // �÷��� ���� ������ ���� ��ȯ
            if (!Is_Grounded())
            {
                // ���� ��ȯ
                m_facingRight = !m_facingRight;

                // ��������Ʈ ���� ����
                transform.localScale = new Vector3(m_facingRight ? 1f : -1f, 1f, 1f);

                // �̵� ���� ���濡 ���� �̵� �ӵ� ������Ʈ
                horizontalInput = m_facingRight ? 1f : -1f;
                m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);
            }
        }

        public void Steal_Item()
        {
            if (m_hasStolenItem == true)
                return;

            m_hasStolenItem = true;
            m_isChasing = true; // �÷��̾ �ѱ� ����
            m_chaseTimer = 0f; // �ѱ� Ÿ�̸� �ʱ�ȭ
            Debug.Log("�������� ���Ѿҽ��ϴ�! �÷��̾ �ѽ��ϴ�.");
            m_playerControl.Stealscore += 1;
            GetComponent<AudioSource>().Play();
        }

        private void Chase_Player()
        {
            if (m_playerTr == null)
                return;

            m_animator.SetBool("A_IsChasing", true);
            // NPC ��ġ���� �÷��̾� ��ġ�� ���ϴ� ���� ����
            Vector3 direction = (m_playerTr.position - transform.position).normalized;

            // NPC�� �÷��̾� ������ �̵�
            m_rigidbody.velocity = new Vector2(direction.x * m_moveSpeed, m_rigidbody.velocity.y);

            // NPC�� �̵� ���� ���� (�÷��̾ �ٶ󺸵���)
            if (direction.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f); // �������� ������
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f); // ������ ������

            m_moveSpeed = 4f;

            // �ѱ� Ÿ�̸� ������Ʈ
            m_chaseTimer += Time.deltaTime;

            // ���� �ð�(10��)�� ������ �ѱ� ����
            if (m_chaseTimer >= m_chaseDuration)
            {
                m_moveSpeed = 3f;
                m_isChasing = false;
                m_hasStolenItem = false; // ���� ������ ���ѱ⸦ ���� �ʱ�ȭ
                Debug.Log("�ѱ⸦ �����մϴ�.");
                m_animator.SetBool("A_IsChasing", false);
            }
        }

        private bool Is_Grounded()
        {
            if (m_facingRight == true)
            {
                Vector2 front = new Vector2(transform.position.x + 1, transform.position.y);
                // �Ʒ� �������� ����ĳ��Ʈ
                RaycastHit2D hit = Physics2D.Raycast(front, Vector2.down, 1.5f, m_groundLayer);
                return hit.collider != null;
            }
            else
            {
                Vector2 back = new Vector2(transform.position.x - 1, transform.position.y);
                // �Ʒ� �������� ����ĳ��Ʈ
                RaycastHit2D hit = Physics2D.Raycast(back, Vector2.down, 1.5f, m_groundLayer);
                return hit.collider != null;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Time.time - m_lastCollisionTime >= m_collisionCooldown)
            {
                if (m_isChasing && collision.gameObject.CompareTag("Player"))
                {
                    PlayerController playerHealth = collision.gameObject.GetComponent<PlayerController>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(1);
                        Debug.Log("HP -1");

                        m_lastCollisionTime = Time.time;
                    }
                }
            }
        }
    }
}
