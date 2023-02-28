using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour,IRestartGame
{
	public Transform m_LookAt;
	public float m_YawRotationalSpeed;
	public float m_PitchRotationalSpeed;
	public float m_MinPitch=-45.0f;
	public float m_MaxPitch=75.0f;
	public KeyCode m_DebugLockAngleKeyCode=KeyCode.I;
	public KeyCode m_DebugLockKeyCode=KeyCode.O;
	public Transform resetPosition;

	[SerializeField] private float m_MinDistanceToLookAt;
	[SerializeField] private float m_MaxDistanceToLookAt;
	[SerializeField] private LayerMask m_RaycastLayerMask;
	[SerializeField] private float m_OffsetOnCollision;

	bool m_AngleLocked=false;
	bool m_CursorLocked=true;
	bool die = false;
	bool reset = false;

	[SerializeField] GameManager gm;

	[Header("BetterCamera")]
	float totalTime;
	[SerializeField] float timeToMoveCamera = 5f;
	Vector3 updatedMousePosition;

	private void OnDestroy()
	{
		gm.removeRestartListener(this);
	}

	void Start()
	{
		Cursor.lockState=CursorLockMode.Locked;
		m_CursorLocked=true;
		gm.addRestartListener(this);
	}
	void OnApplicationFocus()
	{
		if(m_CursorLocked)
			Cursor.lockState=CursorLockMode.Locked;
	}
	void LateUpdate()
	{
        if (!die)
        {
#if UNITY_EDITOR
			if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
				m_AngleLocked = !m_AngleLocked;
			if (Input.GetKeyDown(m_DebugLockKeyCode))
			{
				if (Cursor.lockState == CursorLockMode.Locked)
					Cursor.lockState = CursorLockMode.None;
				else
					Cursor.lockState = CursorLockMode.Locked;
				m_CursorLocked = Cursor.lockState == CursorLockMode.Locked;
			}
#endif




			float l_MouseAxisX = Input.GetAxis("Mouse X");
			float l_MouseAxisY = Input.GetAxis("Mouse Y");

			Vector3 l_Direction = m_LookAt.position - transform.position;
			float l_Distance = l_Direction.magnitude;

			Vector3 l_DesiredPosition = transform.position;

			totalTime += Time.deltaTime;
			if (Input.mousePosition == updatedMousePosition)
			{
				if (totalTime >= timeToMoveCamera)
				{
					reset = true;
					transform.rotation = Quaternion.Lerp(transform.rotation, resetPosition.rotation, 0.01f);
					transform.position = Vector3.Lerp(transform.position, resetPosition.position, 0.01f);
				}
			}
			else { 
				totalTime = 0;
				reset = false;
			}

            if (!reset)
            {
				if (!m_AngleLocked && (l_MouseAxisX > 0.01f || l_MouseAxisX < -0.01f || l_MouseAxisY > 0.01f || l_MouseAxisY < -0.01f))
				{
					Vector3 l_EulerAngles = transform.eulerAngles;
					float l_Yaw = (l_EulerAngles.y + 180.0f);
					float l_Pitch = l_EulerAngles.x;

					//Update Yaw and Pitch
					l_Yaw += m_YawRotationalSpeed * l_MouseAxisX;
					if (l_Pitch > 180.0f) l_Pitch -= 360.0f;
					l_Pitch += m_PitchRotationalSpeed * (-l_MouseAxisY);
					l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch, m_MaxPitch);

					//Update DesiredPosition
					l_Yaw *= Mathf.Deg2Rad;
					l_Pitch *= Mathf.Deg2Rad;
					l_DesiredPosition = m_LookAt.position
												+ new Vector3(
													Mathf.Sin(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance,
													Mathf.Sin(l_Pitch) * l_Distance,
													Mathf.Cos(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance);

					l_Direction = m_LookAt.position - l_DesiredPosition;
				}

				l_Direction /= l_Distance;



				//Clamp between minDistance and maxDistance. Update desiredPosition.
				if (l_Distance > m_MaxDistanceToLookAt || l_Distance < m_MinDistanceToLookAt)
				{
					l_Distance = Mathf.Clamp(l_Distance, m_MinDistanceToLookAt, m_MaxDistanceToLookAt);
					l_DesiredPosition = m_LookAt.position - l_Direction * l_Distance;
				}



				//Bring camera closer if colliding with any object.
				RaycastHit l_RaycastHit;
				Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);
				if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_RaycastLayerMask.value))
				{
					l_DesiredPosition = l_RaycastHit.point + l_Direction * m_OffsetOnCollision;
				}

				transform.forward = l_Direction;
				transform.position = l_DesiredPosition;

				updatedMousePosition = Input.mousePosition;
			}

		}
	
	}

    void IRestartGame.RestartGame()
    {
		die = false;
    }

    void IRestartGame.Die()
    {
		die = true;
    }
}
