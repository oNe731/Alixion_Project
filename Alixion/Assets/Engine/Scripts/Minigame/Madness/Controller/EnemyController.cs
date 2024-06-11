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

        private float m_moveSpeed = 2f; // NPC 이동 속도

        private bool m_facingRight = true;
        private bool m_isChasing = false;
        private bool m_hasStolenItem = false;

        private float m_fallY = -10.0f;
        private float m_TPP = 10.0f;    // 텔레포트 좌표
        private float m_minX = -12.75f; // 이동 가능한 최소 x좌표
        private float m_maxX = 12.75f;  // 이동 가능한 최대 x좌표

        private float m_chaseDuration = 15f; // 쫓기 시간 (10초)
        private float m_chaseTimer = 0f;     // 쫓기 타이머

        private float m_collisionCooldown = 2.0f; // 무적시간
        private float m_lastCollisionTime;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_animator.StartPlayback(); // 애니메이션 정지
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
            // 좌우 이동
            float horizontalInput = m_facingRight ? 1f : -1f;
            m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);

            // 플랫폼 끝에 닿으면 방향 전환
            if (!Is_Grounded())
            {
                // 방향 전환
                m_facingRight = !m_facingRight;

                // 스프라이트 방향 설정
                transform.localScale = new Vector3(m_facingRight ? 1f : -1f, 1f, 1f);

                // 이동 방향 변경에 따라 이동 속도 업데이트
                horizontalInput = m_facingRight ? 1f : -1f;
                m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);
            }
        }

        public void Steal_Item()
        {
            if (m_hasStolenItem == true)
                return;

            m_hasStolenItem = true;
            m_isChasing = true; // 플레이어를 쫓기 시작
            m_chaseTimer = 0f; // 쫓기 타이머 초기화
            Debug.Log("아이템을 빼앗았습니다! 플레이어를 쫓습니다.");
            m_playerControl.Stealscore += 1;
            GetComponent<AudioSource>().Play();
        }

        private void Chase_Player()
        {
            if (m_playerTr == null)
                return;

            m_animator.SetBool("A_IsChasing", true);
            // NPC 위치에서 플레이어 위치로 향하는 방향 벡터
            Vector3 direction = (m_playerTr.position - transform.position).normalized;

            // NPC를 플레이어 쪽으로 이동
            m_rigidbody.velocity = new Vector2(direction.x * m_moveSpeed, m_rigidbody.velocity.y);

            // NPC의 이동 방향 설정 (플레이어를 바라보도록)
            if (direction.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f); // 오른쪽을 보도록
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f); // 왼쪽을 보도록

            m_moveSpeed = 4f;

            // 쫓기 타이머 업데이트
            m_chaseTimer += Time.deltaTime;

            // 일정 시간(10초)이 지나면 쫓기 종료
            if (m_chaseTimer >= m_chaseDuration)
            {
                m_moveSpeed = 3f;
                m_isChasing = false;
                m_hasStolenItem = false; // 다음 아이템 빼앗기를 위해 초기화
                Debug.Log("쫓기를 종료합니다.");
                m_animator.SetBool("A_IsChasing", false);
            }
        }

        private bool Is_Grounded()
        {
            if (m_facingRight == true)
            {
                Vector2 front = new Vector2(transform.position.x + 1, transform.position.y);
                // 아래 방향으로 레이캐스트
                RaycastHit2D hit = Physics2D.Raycast(front, Vector2.down, 1.5f, m_groundLayer);
                return hit.collider != null;
            }
            else
            {
                Vector2 back = new Vector2(transform.position.x - 1, transform.position.y);
                // 아래 방향으로 레이캐스트
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
