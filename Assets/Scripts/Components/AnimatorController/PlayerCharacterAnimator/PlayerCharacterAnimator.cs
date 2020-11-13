using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public sealed class PlayerCharacterAnimator : AnimatorController
{
	[SerializeField] private PlayerCharacter _PlayerCharacter;

	private void Start()
	{
		// 점프 시작시 실행할 내용을 정의합니다.
		_PlayerCharacter.playerMovement.onJumpStarted +=

			// 남은 점프 카운트에 따라 점프 애니메이션을 재생합니다.
			(remainJumpCount) =>
			animator.Play(
				(remainJumpCount > 0) ? "Jump_Single" : "Jump_Multi", 0, 0.0f);

		// 착지시 실행할 내용을 정의합니다.
		_PlayerCharacter.playerMovement.onLanded +=
			(remainJumpCount) =>
			{
				// 만약 남은 점프 카운트가 0 이라면
				if (remainJumpCount == 0)
				{
					// 캐릭터 이동을 중단합니다.
					_PlayerCharacter.playerMovement.StopMove();

					// 착지 애니메이션을 재생합니다.
					animator.Play("Jump_Crouch", 0, 0.0f);
				}
			};
	}

	private void Update()
	{
		SetParam("_VelocityLength", _PlayerCharacter.playerMovement.velociy.magnitude);

		SetParam("_IsInAir", !_PlayerCharacter.playerMovement.isGrounded);

		//animator.SetFloat("_VelocityLength", _PlayerCharacter.playerMovement.velociy.magnitude);
	}

	// PlayerCharacter -> Jump_Crouch 에서 사용중인 메서드입니다.
	private void AnimEvent_CrouchFinished()
	{
		_PlayerCharacter.playerMovement.AllowMove();
	}

}
