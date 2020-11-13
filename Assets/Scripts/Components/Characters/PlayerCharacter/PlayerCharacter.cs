using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private ZoomableSpringArm _SpringArm;

	public ZoomableSpringArm springArm => _SpringArm;

	public PlayerMovement playerMovement { get; private set; }

	public float inputHorizontal { get; private set; }
	public float inputVertical { get; private set; }

	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
	}


	private void Update()
	{
		InputKey();
	}

	private void InputKey()
	{
		inputHorizontal = Input.GetAxisRaw("Horizontal");
		inputVertical = Input.GetAxisRaw("Vertical");
	}

}
