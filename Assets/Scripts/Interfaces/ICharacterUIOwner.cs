using UnityEngine;

// 화면에 이름을 표시하는 UI 를 가질 수 있는 객체들이 구현해야 하는 인터페이스
public interface ICharacterUIOwner
{
	// UI 표시 위치
	Vector3 characterUIPosition { get; }

	// 캐릭터 이름
	string name { get; }

	// CharacterUI 를 소유하는 객체의 Transform
	Transform transform { get; }

}