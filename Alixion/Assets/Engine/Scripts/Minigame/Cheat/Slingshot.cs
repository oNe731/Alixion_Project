namespace Fraud
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Slingshot : MonoBehaviour
    {
        [SerializeField] private LineRenderer[] m_lineRenderers;
        [SerializeField] private Transform[] m_stripPositions;
        [SerializeField] private Transform m_center;
        [SerializeField] private Transform m_idlePosition;

        [SerializeField] private GameObject[] m_birdPrefabs;

        [SerializeField] private bool m_isMouseDown = false;
        [SerializeField] private float m_force = 5f;
        [SerializeField] private float m_maxLength = 3f;
        [SerializeField] private float m_bottomBoundary = -4f;
        [SerializeField] private float m_birdPositionOffset = - 0.4f;

        private Vector3 m_currentPosition = Vector3.zero;
        private AudioSource m_audioSource;
        private Rigidbody2D m_birdRigidbody;
        private Collider2D m_birdCollider;

        private float m_time = 0f;

        void Start()
        {
            m_audioSource = GetComponent<AudioSource>();

            m_lineRenderers[0].positionCount = 2;
            m_lineRenderers[1].positionCount = 2;
            m_lineRenderers[0].SetPosition(0, m_stripPositions[0].position);
            m_lineRenderers[1].SetPosition(0, m_stripPositions[1].position);

            Reset_Strips();
            Create_Bird();
        }

        void Update()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            if(m_birdRigidbody == null)
            {
                m_time += Time.deltaTime;
                if (m_time > 1f)
                {
                    m_time = 0f;
                    Create_Bird();
                }
            }

            if (m_isMouseDown == false)
                return;

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            m_currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (m_currentPosition.y >= m_center.transform.position.y)
                m_currentPosition.y = m_center.transform.position.y;

            m_currentPosition = m_center.position + Vector3.ClampMagnitude(m_currentPosition - m_center.position, m_maxLength);
            m_currentPosition = Clamp_Boundary(m_currentPosition);
            Set_Strips(m_currentPosition);
        }

        private void OnMouseDown()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;
            
            m_isMouseDown = true;
            //Create_Bird();
        }

        private void OnMouseUp()
        {
            if (GameManager.Instance.IsMiniGame == false)
                return;

            if(m_birdRigidbody != null)
                Shoot();

            m_isMouseDown = false;
            m_currentPosition = m_idlePosition.position;
            Reset_Strips();
        }

        void Create_Bird()
        {
            if (m_birdRigidbody != null)
                return;

            m_birdRigidbody = Instantiate(m_birdPrefabs[Random.Range(0, m_birdPrefabs.Length)]).GetComponent<Rigidbody2D>();
            m_birdRigidbody.isKinematic = true;

            m_birdCollider = m_birdRigidbody.GetComponent<Collider2D>();
            m_birdCollider.enabled = false;

            Set_Strips(m_currentPosition);
        }

        void Shoot()
        {
            if (m_audioSource != null)
                m_audioSource.PlayOneShot(m_audioSource.clip);

            if (m_birdCollider)
                m_birdCollider.enabled = true;

            Vector3 birdForce = (m_currentPosition - m_center.position) * m_force * -1;
            m_birdRigidbody.velocity = birdForce;

            m_birdRigidbody = null;
            m_birdCollider = null;
        }

        void Reset_Strips()
        {
            m_currentPosition = m_idlePosition.position;
            Set_Strips(m_currentPosition);
        }

        void Set_Strips(Vector3 position)
        {
            m_lineRenderers[0].SetPosition(1, position);
            m_lineRenderers[1].SetPosition(1, position);

            if (m_birdRigidbody)
            {
                Vector3 dir = position - m_center.position;
                m_birdRigidbody.transform.position = position + dir.normalized * m_birdPositionOffset;
                m_birdRigidbody.transform.right = -dir.normalized;
            }
        }

        Vector3 Clamp_Boundary(Vector3 vector)
        {
            vector.y = Mathf.Clamp(vector.y, m_bottomBoundary, 1000);
            return vector;
        }

        public void Change_Bird(GameObject newBirdPrefab)
        {
            if (m_birdRigidbody == null)
                return;

            Destroy(m_birdRigidbody.gameObject);

            m_birdRigidbody = Instantiate(newBirdPrefab).GetComponent<Rigidbody2D>();
            m_birdRigidbody.isKinematic = true;

            m_birdCollider = m_birdRigidbody.GetComponent<Collider2D>();
            m_birdCollider.enabled = false;

            Set_Strips(m_currentPosition);
        }
    }
}

