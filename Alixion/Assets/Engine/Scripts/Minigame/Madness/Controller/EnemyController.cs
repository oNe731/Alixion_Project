namespace Madness
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private PlayerController m_playerControl;
        [SerializeField] private LayerMask m_groundLayer;
        [SerializeField] private LayerMask m_objectLayer;

        private Transform   m_playerTr;
        private Animator    m_animator;
        private Rigidbody2D m_rigidbody;
        private SpriteRenderer m_spriteRenderer;

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

        private bool m_Jump = false;
        private float m_jumpTime = 0f;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_animator.StartPlayback(); // �ִϸ��̼� ����
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_spriteRenderer = GetComponent<SpriteRenderer>();

            m_lastCollisionTime = -m_collisionCooldown;
            m_playerTr = m_playerControl.gameObject.transform;
        }

        void Update()
        {
            if (GameManager.Instance.IsMiniGame == false || GameManager.Instance.Pause == true)
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
            if(m_Jump == true)
            {
                m_jumpTime += Time.deltaTime;
                if(m_jumpTime > 0.1f)
                {
                    if(Is_Grounded(0f, 0.6f))
                        m_Jump = false;
                }
            }
            else if (m_Jump == false && Is_Jump(0.4f, 2.0f)) // ���� ���� ���ʿ� ���� ������ ����
            {
                if (!Is_Grounded(0f, 0.6f))
                    return;

                m_Jump = true;
                m_jumpTime = 0f;
                m_rigidbody.velocity = Vector2.zero;
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 5f);
            }
            else if (!Is_Grounded(0.4f, 4.0f) || Is_Front()) // �տ� ������Ʈ�� ������ || �÷��� ���� ������ ���� ��ȯ
            {
                if (m_Jump == true || !Is_Grounded(0f, 0.6f))
                    return;

                // ���� ��ȯ
                m_facingRight = !m_facingRight;

                // ��������Ʈ ���� ����
                m_spriteRenderer.flipX = !m_facingRight;
            }

            // �¿� �̵�
            float horizontalInput = m_facingRight ? 1f : -1f;
            m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);

            transform.GetChild(0).gameObject.transform.localPosition = new Vector3(horizontalInput * 0.5f, transform.GetChild(0).gameObject.transform.localPosition.y, transform.GetChild(0).gameObject.transform.localPosition.z);
        }

        public void Steal_Item()
        {
            if (m_hasStolenItem == true)
                return;

            transform.GetChild(0).gameObject.SetActive(false);
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
            {
                m_spriteRenderer.flipX = false;
                transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0.5f, transform.GetChild(0).gameObject.transform.localPosition.y, transform.GetChild(0).gameObject.transform.localPosition.z);
            }
            else if (direction.x < 0)
            {
                m_spriteRenderer.flipX = true;
                transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-0.5f, transform.GetChild(0).gameObject.transform.localPosition.y, transform.GetChild(0).gameObject.transform.localPosition.z);
            }

            m_moveSpeed = 4f;

            // �ѱ� Ÿ�̸� ������Ʈ
            m_chaseTimer += Time.deltaTime;

            // ���� �ð�(10��)�� ������ �ѱ� ����
            if (m_chaseTimer >= m_chaseDuration)
                Stop_Chase();
        }

        public void Stop_Chase()
        {
            if (m_isChasing == false)
                return;

            m_isChasing = false;
            m_hasStolenItem = false; // ���� ������ ���ѱ⸦ ���� �ʱ�ȭ
            transform.GetChild(0).gameObject.SetActive(true);
            m_animator.SetBool("A_IsChasing", false);
            m_moveSpeed = 3f;
        }

        private bool Is_Jump(float Xdistance = 0.3f, float Ydistance = 0.7f)
        {
            float direction = m_facingRight ? 1f : -1f;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + (Xdistance * direction), transform.position.y), Vector2.up, Ydistance, m_groundLayer);
            //Debug.DrawRay(new Vector2(transform.position.x + (Xdistance * direction), transform.position.y), Vector2.up * Ydistance, Color.red);

            return hit.collider != null;
        }

        private bool Is_Front(float Xdistance = 0.7f)
        {
            Vector2 direction = m_facingRight ? Vector2.right : Vector2.left;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, Xdistance, m_objectLayer);

            // Debug.DrawRay(transform.position, direction * Xdistance, Color.red);

            RaycastHit2D closestHit = default;
            float closestDistance = float.MaxValue;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != this.gameObject)
                {
                    float distance = Vector2.Distance(transform.position, hit.point);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestHit = hit;
                    }
                }
            }

            return closestHit.collider != null;
        }

        private bool Is_Grounded(float Xdistance = 0.3f, float Ydistance = 1f)
        {
            float direction = m_facingRight ? 1f : -1f;

            Vector2 Start = new Vector2(transform.position.x + (Xdistance * direction), transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(Start, Vector2.down, Ydistance, m_groundLayer);
            Debug.DrawRay(Start, Vector2.down * Ydistance, Color.red);

            return hit.collider != null;
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
