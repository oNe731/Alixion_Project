namespace Madness
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class MoveController : MonoBehaviour
    {
        [SerializeField] private Button m_leftButton;
        [SerializeField] private Button m_rightButton;
        [SerializeField] private Button m_jumpButton;

        private float m_moveSpeed = 4f;
        private float m_jumpForce = 10f;
        private bool m_isMovingLeft = false;
        private bool m_isMovingRight = false;
        private bool m_isGrounded = true;

        private Rigidbody2D m_rigidbody;

        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (m_leftButton != null)
            {
                EventTrigger trigger = m_leftButton.gameObject.AddComponent<EventTrigger>();
                Add_EventTrigger(trigger, EventTriggerType.PointerDown, () => Move_Left(true));
                Add_EventTrigger(trigger, EventTriggerType.PointerUp, () => Move_Left(false));
            }

            if (m_rightButton != null)
            {
                EventTrigger trigger = m_rightButton.gameObject.AddComponent<EventTrigger>();
                Add_EventTrigger(trigger, EventTriggerType.PointerDown, () => Move_Right(true));
                Add_EventTrigger(trigger, EventTriggerType.PointerUp, () => Move_Right(false));
            }

            if (m_jumpButton != null)
                m_jumpButton.onClick.AddListener(Jump);
        }

        public void Update()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            Move();
        }

        void FixedUpdate()
        {
            Check_Ground();
        }

        public void Move()
        {
            float horizontalInput = 0f;

            if (m_isMovingLeft)
                horizontalInput = -1f;
            else if (m_isMovingRight)
                horizontalInput = 1f;

            m_rigidbody.velocity = new Vector2(horizontalInput * m_moveSpeed, m_rigidbody.velocity.y);
        }

        public void Move_Left(bool isPressed)
        {
            m_isMovingLeft = isPressed;
        }

        public void Move_Right(bool isPressed)
        {
            m_isMovingRight = isPressed;
        }

        void Jump()
        {
            if (m_isGrounded == false)
                return;

            m_isGrounded = false;
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_jumpForce);

            GetComponent<AudioSource>().Play();
        }

        void Check_Ground()
        {
            // 캐릭터 아래쪽으로 레이캐스트를 쏘기 위한 시작점과 방향 설정
            Vector2 origin = transform.position;
            Vector2 direction = Vector2.down;
            float distance = 1.0f;

            // 레이캐스트를 통해 충돌 정보 가져오기
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Ground"));

            // 레이를 시각적으로 표시 (디버깅용)
            Debug.DrawRay(origin, direction * distance, Color.red);

            // 레이가 바닥과 충돌했는지 여부로 isGrounded 판단
            m_isGrounded = (hit.collider != null);
        }

        private void Add_EventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener((eventData) => { action(); });
            trigger.triggers.Add(entry);
        }
    }
}
