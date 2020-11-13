using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animator 컴포넌트를 제어하기 위한 클래스
public abstract class AnimatorController : MonoBehaviour
{
	// 제어할 Animator 컴포넌트를 나타냅니다.
	[SerializeField] private Animator _Animator;

	public Animator animator => _Animator;


	// Animator Parameter 값을 설정합니다.
	public void SetParam<T>(string paramName, T value) where T : struct
	{
		switch (value)
		{
			case int i:
				_Animator.SetInteger(paramName, i);
				break;

			case float f:
				_Animator.SetFloat(paramName, f);
				break;

			case bool b:
				_Animator.SetBool(paramName, b);
				break;
		}
	}

	// Animator Trigger 를 설정합니다.
	public void SetTrigger(string triggerName)
	{
		_Animator.SetTrigger(triggerName);
	}


	// Animator Parameter 값을 얻습니다.
	public T GetParam<T>(string paramName) where T : struct
	{
		switch(typeof(T).Name)
		{
			case "Int32":	return (T)Convert.ChangeType(_Animator.GetInteger(paramName), typeof(T));
			case "Single":	return (T)Convert.ChangeType(_Animator.GetFloat(paramName), typeof(T));
			case "Boolean":	return (T)Convert.ChangeType(_Animator.GetBool(paramName), typeof(T));
			default: throw new Exception("사용 가능한 타입이 아닙니다.");
		}
	}

 
}
