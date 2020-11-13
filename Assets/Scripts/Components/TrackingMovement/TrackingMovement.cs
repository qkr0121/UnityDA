using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class TrackingMovement : MonoBehaviour
{
	[Header("Use Smooth Tracking")]
	[Tooltip("부드러운 추적을 사용합니다.")]
	[SerializeField] protected bool m_UseSmoothTracking = true;

	[Header("Smooth Tracking Speed")]
	[Tooltip("부드러운 추적 속력")]
	[SerializeField] protected float m_SmoothTrackingSpeed = 10.0f;

	[Header("추적 속력")]
	[SerializeField] protected float m_TrackingSpeed = 10.0f;

	[Header("추적 이동 사용")]
	[SerializeField] protected bool m_UseTrackingMovement = true;

	[Header("Offset")]
	[SerializeField] protected Vector3 m_Offset;

	[Header("추적 타깃")]
	[Tooltip("UseTrackingTargetParent 가 true 일 경우 자동으로 설정됩니다.")]
	[SerializeField] protected Transform m_TrackingTarget;

	[Header("타깃을 부모 오브젝트로")]
	[SerializeField] protected bool m_UseTrackingTargetParent = false;

	[Header("이 오브젝트를 루트 오브젝트로")]
	[Tooltip("해당 오브젝트를 최상위 오브젝트로 설정합니다.")]
	[SerializeField] protected bool m_IsRootObject = false;

	public Transform trackingTarget
	{ get => m_TrackingTarget; set => m_TrackingTarget = value; }

	public float trackingSpeed 
	{ get => m_TrackingSpeed; set => m_TrackingSpeed = value; }

	public bool useTrackingMovement 
	{ get => m_UseTrackingMovement; set => m_UseTrackingMovement = value; }

	protected virtual void Awake()
	{
		// _UseTrackingTargetParent 가 true 일 경우 추적 목표를 부모 오브젝트로 설정합니다.
		if (m_UseTrackingTargetParent)
			trackingTarget = transform.parent;
		/// - transform.parent : 해당 오브젝트의 부모 오브젝트를 나타냅니다.

		// _IsRootObject 가 true 일 경우 해당 오브젝트를 최상위 오브젝트로 설정합니다.
		if (m_IsRootObject)
			transform.SetParent(null);
	}

	protected virtual void FixedUpdate()
	{
		if (m_UseTrackingMovement)
		{
			TrackingTarget();
		}
		
	}


	// 목표 오브젝트를 추적합니다.
	private void TrackingTarget()
	{
		// 타깃이 존재하지 않는 경우 추적을 실행하지 않습니다.
		if (trackingTarget == null) return;

		transform.position =
			(m_UseSmoothTracking) ?

			Vector3.Lerp(
				transform.position,
				m_TrackingTarget.position + m_Offset,
				m_TrackingSpeed * Time.deltaTime) :

			m_TrackingTarget.position + m_Offset;
	}

	// 목표 추적 이동이 끝났는지를 확인합니다.
	public bool CheckTrackingFinished()
	{
		// 추적 상태가 아니라면 false 를 리턴합니다.
		if (!m_UseTrackingMovement) return false;

		// 추적 목표가 존재하지 않는다면 false 를 리턴합니다.
		else if (m_TrackingTarget == null) return false;

		// 목표와의 거리가 1.0 미만이라면 true 를 리턴합니다.
		return Vector3.Distance(
			m_TrackingTarget.position + m_Offset, transform.position) < 1.0f;
	}
}
