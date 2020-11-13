﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour
{
	[Header("최대 이동 속력")]
	[SerializeField] private float _MaxSpeed = 6.0f;

	[Header("1초 가속률")]
	[Range(100.0f, 10000.0f)]
	[Tooltip("1초에 걸쳐 최대 속력의 몇 퍼센트만큼 가속되도록 할 것인지를 결정합니다.")]
	/// - 100% = 최대 속력까지 1초
	/// - 500% = 최대 속력까지 0.2초
	/// - 1000% = 최대 속력까지 0.1초
	[SerializeField] private float _OneSecAcceleration = 600.0f;

	[Header("Orient Rotation To Movement")]
	[Tooltip("이동하는 방향으로 회전시킵니다.")]
	[SerializeField] private bool _UseOrientRotationToMovement = true;

	[Header("Yaw 회전 속력")]
	[Tooltip("Orient Rotation To Movement 속성과 함께 사용됩니다.")]
	[SerializeField] private float _RotationYawSpeed = 720.0f;

	[Header("적용되는 중력 스케일")]
	[Space(30.0f)]
	[SerializeField] private float _GravityScale = 1.0f;

	[Header("최대 Y 속력")]
	[SerializeField] private float _MaxYVelocity = 10.0f;

	[Header("최대 점프 카운트")] [Space(30.0f)]
	[SerializeField] private int _MaxJumpCount = 2;


	[Header("점프 힘")]
	[SerializeField] private float _JumpVelocityY = 10.0f;

	[Header("지형 검사시 무시할 레이어")]
	[SerializeField] private LayerMask _IgnoreGroundLayers;


	private PlayerCharacter _PlayerCharacter;

	// 이동 입력값을 나타냅니다.
	private Vector3 _InputVector;

	// 목표 속도를 저장합니다.
	private Vector3 _TargetVelocity;

	// 속도를 나타냅니다.
	private Vector3 _Velocity;

	// 충격 속도를 저장합니다.
	private Vector3 _ImpulseVelocity;

	// 이동 가능 상태를 나타냅니다.
	private bool _IsMovable = true;

	// 점프를 시작한 후 3초가 지난 후 onLanded 대리자를 호출합니다.
	private IEnumerator _WaitJumpMaxDelay;

	// 3 초를 대기시킵니다.
	private WaitForSeconds _Wait3Sec = new WaitForSeconds(3.0f);

	// 남은 점프 카운트
	///  - 해당 값이 0 이라면 점프할 수 없도록 합니다.
	private int _RemainJumpCount;

	// 점프키 입력 끝 상태를 나타냅니다.
	private bool _IsJumpInputFinished = true;

	// 이전 땅 착지 상태를 나타냅니다.
	private bool _PrevGroundedState;

	// 땅으로 착지시 호출되는 대리자
	///  - param : 남은 점프 카운트
	public System.Action<int> onLanded;

	// 점프 시작시 호출되는 대리자
	///  - param : 남은 점프 카운트
	public System.Action<int> onJumpStarted;



	// 사용되는 CharacterController 컴포넌트를 나타냅니다.
	public CharacterController characterController { get; private set; }

	// 땅에 닿음 상태를 나타냅니다.
	public bool isGrounded
	{ get; private set; }

	// 속도를 나타냅니다.
	public Vector3 velociy => characterController.velocity;

	// 점프 가능 상태를 나타냅니다.
	public bool isJumpable =>

		// 이동 가능 상태이며
		_IsMovable

		// 땅에 닿아있거나, 남은 점프 카운트가 0 보다 크고
		&& (isGrounded || (_RemainJumpCount > 0))

		// 점프 입력이 끝났을 경우 점프할 수 있도록 합니다.
		&& _IsJumpInputFinished;



	private void Awake()
	{
		_PlayerCharacter = GetComponent<PlayerCharacter>();
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		// 캐릭터를 이동 방향으로 회전시킵니다.
		OrientRotationToMovement();

		if (Input.GetKeyDown(KeyCode.LeftShift))
			AddImpulseMovemenet(transform.forward * 5.0f, false);
		if (Input.GetKeyDown(KeyCode.RightShift))
			AddImpulseMovemenet(transform.forward * 5.0f, true);
	}


	private void FixedUpdate()
	{

		// 이동 입력값을 초기화
		_InputVector = Vector3.zero;

		// 이동 입력 값을 저장합니다.
		_InputVector.x = _PlayerCharacter.inputHorizontal;
		_InputVector.z = _PlayerCharacter.inputVertical;

		// 이동 입력값을 카메라 방향으로 변환합니다.
		_InputVector = _PlayerCharacter.springArm.InputToCameraDirection(_InputVector);

		// 점프 입력 처리를 합니다.
		if (Input.GetKey(KeyCode.Space))
			JumpInput();
		else FinishJumpInput();

		// 속도를 계산합니다.
		CalculateSpeed();

		// 중력을 계산합니다.
		CalculateGravity();

		// 캐릭터를 이동시킵니다.
		characterController.Move(_Velocity);

		// 땅 착지 상태를 업데이트 합니다.
		UpdateGroundedState();
	}

	// 속도를 계산합니다.
	private void CalculateSpeed()
	{
		// 입력 값을 연산합니다.
		void CalculateInputVector()
		{
			// 속도에 이동 입력값과 속력을 연산합니다.
			_TargetVelocity.x = _InputVector.x * _MaxSpeed * Time.deltaTime;
			_TargetVelocity.z = _InputVector.z * _MaxSpeed * Time.deltaTime;
		}

		// 가속률을 연산합니다.
		void CalculateOneSecAcceleration()
		{
			// 현재 Y 속력을 저장합니다.
			float currentVelocityY = _TargetVelocity.y;

			// 목표 속도를 저장합니다.
			Vector3 currentVelocity = (_IsMovable) ? _TargetVelocity : Vector3.zero;

			// 현재 속도를 저장합니다.
			_Velocity = characterController.velocity * Time.deltaTime;

			// 현재 속도와, 목표 속도에서 y 축 값을 제외합니다.
			_Velocity.y = currentVelocity.y = 0.0f;

			// 가속률을 연산시킵니다.
			_Velocity = Vector3.MoveTowards(
				_Velocity,
				currentVelocity,
				_MaxSpeed * (_OneSecAcceleration * 0.01f * Time.deltaTime) * Time.deltaTime);


			// 충격 이동 속도를 연산합니다.
			_Velocity += _ImpulseVelocity * Time.deltaTime;

			// 충격 이동 속도가 서서히 감소하도록 합니다.
			_ImpulseVelocity = Vector3.MoveTowards(
				_ImpulseVelocity,
				Vector3.zero,
				_MaxSpeed * 10.0f * Time.deltaTime);

			// 연산된 속도에 Y 속력을 적용시킵니다.
			_Velocity.y = currentVelocityY;
		}

		// 입력 값을 연산합니다.
		CalculateInputVector();

		// 가속률을 연산합니다.
		CalculateOneSecAcceleration();

	}

	// 중력을 계산합니다.
	private void CalculateGravity()
	{
		// Y 축 속력에 중력을 적용시킵니다.
		_TargetVelocity.y += Physics.gravity.y * _GravityScale * 0.05f * Time.deltaTime;
		/// - Physics.gravity : 엔진에 설정된 중력을 나타냅니다.

		// Y 축 이동 속력이 최대 Y 축 이동 속력을 초과하지 않도록 합니다.
		_TargetVelocity.y = Mathf.Clamp(_TargetVelocity.y, -_MaxYVelocity, _MaxYVelocity);
	}

	// 땅에 닿음 상태를 업데이트합니다.
	private void UpdateGroundedState()
	{

		Ray ray = new Ray(
			transform.position + characterController.center,
			Vector3.down);

		RaycastHit hit;

		// 땅에 닿아있는지 확인합니다.
		isGrounded = 
			
			// 캐릭터 레이어가 아닌, 다른 오브젝트가 하단에 존재하며,
			Physics.SphereCast(ray,

			// 구의 반지름을 설정합니다.
			characterController.radius,

			out hit,
			// (캐릭터 캡슐의 중간 높이) + (skinWidth * 2.0f) - (구의 반지름) 만큼으로 길이를 설정합니다.
			characterController.center.y + (characterController.skinWidth * 2) - characterController.radius,

			// Sphere Cast 의 타깃 레이어를 설정합니다.
			~_IgnoreGroundLayers) &&

			// 캐릭터가 상승중이 아닐 경우
			_Velocity.y <= 0.0f;

		if (isGrounded)
		{
			// 땅에 닿아있다면 y 축 이동 속도를 0 으로 설정합니다.
			_TargetVelocity.y = 0.0f;
		}

		// 땅에 닿음 상태가 이전과 다르다면
		if (_PrevGroundedState != isGrounded)
		{
			// 이전 상태가 땅에 닿아있음 상태일 경우
			if (_PrevGroundedState)
			{
				// 남은 점프 카운트가 _MaxJumpCount 와 동일한 경우
				// 지형에서 떨어지는 상태이므로 점프 카운트를 감소시키며, 점프 상태로 설정합니다.
				if (_RemainJumpCount == _MaxJumpCount && _IsMovable)
				{
					// 점프 카운트 1 감소
					--_RemainJumpCount;

					// 하단으로 살짝 이동시킵니다.
					_TargetVelocity.y -= Time.deltaTime;

					// 점프 시작 이벤트 실행
					onJumpStarted?.Invoke(_RemainJumpCount);
				}

			}
			// 이전 상태가 땅에 닿지 않음 상태일 경우
			else
			{
				// 착지 이벤트를 실행
				onLanded?.Invoke(_RemainJumpCount);

				// 남은 점프 카운트를 초기화합니다.
				_RemainJumpCount = _MaxJumpCount;
			}

			// 상태를 업데이트합니다.
			_PrevGroundedState = isGrounded;
		}



	}

	// 점프 입력을 시작합니다.
	private void JumpInput()
	{
<<<<<<< HEAD
=======
		// 점프를 시작한 후 3초가 지난 후 onLanded 대리자를 호출합니다.
		IEnumerator WaitJumpMaxDelay()
		{
			yield return _Wait3Sec;
			AllowMove();
			_WaitJumpMaxDelay = null;
		}
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0

		// 점프 가능 상태일 경우
		if (isJumpable)
		{
			// 남은 점프 카운트가 0 보다 클 경우, 1 감소시킵니다.
			if (_RemainJumpCount > 0) --_RemainJumpCount;

			// Y 이동 속력을 변경합니다.
			_TargetVelocity.y = _JumpVelocityY * Time.deltaTime;

			// 점프 시작 이벤트 실행
			onJumpStarted?.Invoke(_RemainJumpCount);

<<<<<<< HEAD
			
=======
			if (_WaitJumpMaxDelay != null)
				StopCoroutine(_WaitJumpMaxDelay);

			StartCoroutine(_WaitJumpMaxDelay = WaitJumpMaxDelay());
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
		}

		// 점프키가 입력되었으므로, 점프 입력 상태로 설정합니다.
		_IsJumpInputFinished = false;

	}

	// 점프 입력을 끝냅니다.
	private void FinishJumpInput()
	{
		_IsJumpInputFinished = true;
	}

	// 이동하는 방향으로 캐릭터를 회전시킵니다.
	private void OrientRotationToMovement()
	{
		// _UseOrientRotationToMovement 속성을 사용하지 않는다면 실행하지 않습니다.
		if (!_UseOrientRotationToMovement) return;

		// 이동 불가능 상태라면 실행하지 않습니다.
		if (!_IsMovable) return;

		// 이동 입력이 없을 경우 실행하지 않습니다.
		if (_InputVector.magnitude <= characterController.minMoveDistance) return;

		// 목표 회전값을 저장합니다.
		float targetYawRotation = _TargetVelocity.ToAngle(true);

		// 현재 회전값을 저장합니다.
		float currentYawRotation = transform.eulerAngles.y;

		// 다음 회전값을 저장합니다.
		float nextYawRotation = Mathf.MoveTowardsAngle(
			currentYawRotation, targetYawRotation, _RotationYawSpeed * Time.deltaTime);

		// 적용시킬 오일러각을 저장합니다.
		Vector3 eulerAngle = Vector3.up * nextYawRotation;

		transform.eulerAngles = eulerAngle;
	}



	// 축격 이동을 추가합니다.
	///  - prevImpulseIgnore : 이전 충격 속도를 무시합니다.
	public void AddImpulseMovemenet(Vector3 impulseVelocity, bool prevImpulseIgnore = false)
	{
		if (prevImpulseIgnore)
			_ImpulseVelocity = impulseVelocity;

		else _ImpulseVelocity += impulseVelocity; 
	}

	// 이동을 멈춥니다.
	public void StopMove()
	{
		_IsMovable = false;
	}

	// 이동을 허용합니다.
	public void AllowMove()
	{
		Debug.Log("AllowMove");
		_IsMovable = true;
	}

}
