using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public float m_MaximumMagnitude = 1f;

	public float m_TranslationShakeSpeed = 0.01f;

	public float m_RotationShakeSpeed = 0.01f;

	public float m_DecreaseRate = 1f;

	private float m_CurrentMagnitude;

	private Transform m_CachedTrans;

	private Vector3 m_OriginalPosition;

	private Quaternion m_OriginalOrientation;

	private void Start()
	{
		m_CachedTrans = base.transform;
		m_OriginalPosition = m_CachedTrans.position;
		m_OriginalOrientation = m_CachedTrans.rotation;
	}

	private void LateUpdate()
	{
		if (m_CurrentMagnitude > 0f)
		{
			m_CachedTrans.rotation = Quaternion.Lerp(m_CachedTrans.rotation, Random.rotation, m_RotationShakeSpeed * m_CurrentMagnitude);
			m_CurrentMagnitude -= m_DecreaseRate * Time.deltaTime;
		}
		else
		{
			m_CurrentMagnitude = 0f;
			base.enabled = false;
		}
	}

	public void IncreaseMagnitude(float amount)
	{
		m_CurrentMagnitude += amount;
		m_CurrentMagnitude = Mathf.Min(m_CurrentMagnitude, m_MaximumMagnitude);
	}

	public void Shake()
	{
		m_CurrentMagnitude = m_MaximumMagnitude;
		base.enabled = true;
	}
}
