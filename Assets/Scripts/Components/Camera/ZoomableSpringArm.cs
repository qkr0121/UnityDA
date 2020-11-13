using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ZoomableSpringArm : TrackingMovement
{
	[Header("Spring Arm 최대 길이")]
	[SerializeField] private float _ArmLengthMax = 7.0f;

	[Header("Spring Arm 최소 길이")]
	[SerializeField] private float _ArmLengthMin = 3.0f;

	[Header("Spring Arm 길이")]
	[SerializeField] private float _ArmLength = 5.0f;

	[Header("컬리전 테스트시 무시할 레이어")]
	[SerializeField] private LayerMask _LayerToIgnore;

	[Header("Max Pitch Value")]
	[SerializeField] private float _MaxPitchValue = 60.0f;

	[Header("Rotation Speed")]
	[SerializeField] private float _RotationSpeed = 2.0f;

	private float _CurrentArmLength;

	private float _PitchRotation;
	private float _YawRotation;

	public new Camera camera { get; private set; }


	protected override void Awake()
	{
		base.Awake();

		camera = GetComponentInChildren<Camera>();
		/// - GetComponentInChildren<Component>() : 자신부터 자식까지 Component 형식과 일치하는 
		///   Component 를 찾아 반환합니다.

		camera.transform.localPosition = Vector3.back * _ArmLength;
	}


	private void Update()
	{
		ZoomCamera(Input.GetAxisRaw("Mouse ScrollWheel"));

		AddYaw(Input.GetAxisRaw("Mouse X"));
		AddPitch(Input.GetAxisRaw("Mouse Y"));

		// 카메라 회전
		RotationArm();

	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		DoCollisionTest();
	}

	// 레이캐스팅을 이용하여 카메라와 캐릭터간의 충돌체가 존재하는지 확인합니다.
	private void DoCollisionTest()
	{
		// 레이를 쏠 방향
		Vector3 rayDirection = transform.position.Direction(camera.transform.position);

		// 레이의 시작점, 방향 정의
		Ray ray = new Ray(transform.position, rayDirection);

		// 레이캐스트 결과를 저장할 변수를 선언합니다.
		RaycastHit hit;

		// 만약 레이에 맞은 충돌체가 존재한다면
		if (Physics.Raycast(ray, out hit, _ArmLength, ~_LayerToIgnore))
		{
			// 충돌체 앞으로 카메라 길이를 설정합니다.
			_CurrentArmLength = hit.distance;
		}

		// 만약 레이에 맞은 충돌체가 존재하지 않는다면
		else
		{
			// 카메라 거리를 기본 거리로 설정합니다.
			_CurrentArmLength = _ArmLength;
		}

#if UNITY_EDITOR
		// Scene 에서 광선 경로를 표시합니다.
		Debug.DrawRay(ray.origin, ray.direction * _CurrentArmLength, Color.red);
#endif


		// 캐릭터와 카메라의 거리를 조절합니다.
		camera.transform.localPosition = Vector3.back * _CurrentArmLength;
	}

	// 카메라 길이를 조절합니다.
	private void ZoomCamera(float value)
	{
		_ArmLength += value;

		_ArmLength = Mathf.Clamp(_ArmLength, _ArmLengthMin, _ArmLengthMax);
	}

	// _YawRotation, _PitchRotation 값을 이용하여 SpringArm 을 회전시킵니다.
	private void RotationArm()
	{
		transform.eulerAngles = new Vector3(_PitchRotation, _YawRotation, 0.0f);
	}

	// Pitch 회전값을 증가시킵니다.
	public void AddPitch(float value)
	{
		_PitchRotation -= value * _RotationSpeed;
		_PitchRotation = Mathf.Clamp(_PitchRotation, -_MaxPitchValue, _MaxPitchValue);
	}

	// Yaw 회전값을 증가시킵니다.
	public void AddYaw(float value)
	{
		_YawRotation += value * _RotationSpeed;
	}

	// 카메라가 보는 방향으로 입력 값을 변환하여 반환합니다.
	public Vector3 InputToCameraDirection(Vector3 inputDirection)
	{
		// 카메라의 앞, 오른쪽 방향을 저장합니다.
		Vector3 cameraForward = camera.transform.forward;
		Vector3 cameraRight = camera.transform.right;

		cameraForward.y = 0.0f;
		cameraForward.Normalize();

		// 각각 방향에 입력 값을 연산합니다.
		cameraForward *= inputDirection.z;
		cameraRight *= inputDirection.x;

		// 만들어진 방향을 정규화하여 반환합니다.
		return (cameraForward + cameraRight).normalized;
	}

}
