using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerClassBase<PlayerManager>
{
<<<<<<< HEAD
    // 플레이어 캐릭터 객체를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // 플레이어 캐릭터에 대한 읽기 전용 프로퍼티
    public PlayerCharacter playerCharacter => _PlayerCharacter =
        _PlayerCharacter ?? GameObject.Find("PlayerCharacter").GetComponent<PlayerCharacter>();

    // 화면에 표시되는 UI 를 나타냅니다.
    public GameUI gameUI { get; private set; }

    public override void InitializeManagerClass()
    {

    }

    public override void OnSceneChanged(string newSceneName)
    {
        gameUI = Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
            "GameUI",
            "Prefabs/UI/GameUI/GameUI").GetComponent<GameUI>());
    }
=======
	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	// 플레이어 캐릭터에 대한 읽기 전용 프로퍼티
	public PlayerCharacter playerCharacter => _PlayerCharacter = _PlayerCharacter ?? 
		GameObject.Find("PlayerCharacter").GetComponent<PlayerCharacter>();

	public override void InitializeManagerClass()
	{
	}
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
}
