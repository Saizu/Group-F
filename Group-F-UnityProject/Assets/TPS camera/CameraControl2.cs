using UnityEngine;
using System.Linq;

namespace Complete
{
    public class CameraControl2 : MonoBehaviour
    {
        public float m_DampTime = 0f;
        public float m_ScreenEdgeBuffer = 1f;
        public float m_MinSize = 3f;
        public float m_CameraOffset = 2f;
        public float m_HeightOffset = 1f;
        public Vector3 m_InitialRotation = new Vector3(20f, 0f, 0f); // Updated initial rotation
        [HideInInspector] public Transform[] m_Targets;
        private Camera m_Camera;
        private Vector3 m_MoveVelocity;
        private Vector3 m_DesiredPosition;

        private void Awake()
        {
            m_Camera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            SetTargetForPlayerOne();
            SetStartPositionAndSize();
            SetInitialRotation();
            SetCameraToDesiredPositionAndRotation(); // New method to set camera position and rotation
        }

        private void FixedUpdate()
        {
            Move();
            Zoom();
        }

        private void Move()
        {
            FindDesiredPosition();
            transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
            transform.rotation = Quaternion.LookRotation(m_Targets[0].forward, Vector3.up);
        }

        private void FindDesiredPosition()
        {
            if (m_Targets.Length > 0)
            {
                Vector3 tankPosition = m_Targets[0].position;
                m_DesiredPosition = tankPosition - m_Targets[0].forward * m_CameraOffset;
                m_DesiredPosition.y = tankPosition.y + m_HeightOffset;
            }
        }

        private void Zoom()
        {
            // Zoom logic goes here
        }

        public void SetStartPositionAndSize()
        {
            FindDesiredPosition();
            transform.position = m_DesiredPosition;
        }

        private void SetInitialRotation()
        {
            transform.eulerAngles = m_InitialRotation;
        }

        private void SetTargetForPlayerOne()
        {
            GameObject playerOneTank = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault(t => t.GetComponent<TankMovement>().m_PlayerNumber == 1);
            if (playerOneTank != null)
            {
                m_Targets = new Transform[] { playerOneTank.transform };
            }
        }

        private void SetCameraToDesiredPositionAndRotation()
        {
            m_Camera.transform.localPosition = new Vector3(0f, 10f, -15f); // Set the desired camera position
            m_Camera.transform.localEulerAngles = new Vector3(20f, 0f, 0f); // Set the desired camera rotation
        }
    }
}