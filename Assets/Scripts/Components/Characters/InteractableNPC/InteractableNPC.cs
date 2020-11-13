using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class InteractableNPC : MonoBehaviour, ICharacterUIOwner
{
	[Header("NPC 코드")]
	[SerializeField] private string _NPCCode;

	// NPC 정보를 나타냅니다.
	private NPCInfo _NPCInfo;

	// NPC 의 영역을 나타냅니다.
	private CapsuleCollider _CapsuleCollider;

	// 캐릭터 머리 위에 띄우는 UI 위치를 나타냅니다.
	public Vector3 characterUIPosition { get; private set; }

	public new string name => _NPCInfo.npcName;

	private void Awake()
	{
		_CapsuleCollider = GetComponent<CapsuleCollider>();
		_CapsuleCollider.isTrigger = true;

		// NPC 정보 로드
		LoadNPCInfo();

		characterUIPosition = transform.position + 
			((_CapsuleCollider.height) * Vector3.up);
	}

	private void LoadNPCInfo()
	{
		_NPCInfo.npcCode = "90000";
		_NPCInfo.npcName = "대장장이";
		_NPCInfo.dialogMessages = new string[]
		{
			"반갑습니다.",
			"Hello!",
			"Bye, Bye"
		};

		ResourceManager.Instance.SaveJson<NPCInfo>(
			_NPCInfo,
			"NPCInfos",
			_NPCInfo.npcCode);
	}

	private void Start()
	{
		PlayerManager.Instance.gameUI.characterUIDrawer.CreateCharacterWidget(this);
	}
}
