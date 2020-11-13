using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CharacterUIDrawer : MonoBehaviour
{
<<<<<<< HEAD
    private CharacterUI _CharacterUIPrefab;

    private ObjectPool<CharacterUI> _CharacterUIPool = new ObjectPool<CharacterUI>();

    public void CreateCharacterWidget(ICharacterUIOwner owner)
    {
        if(!_CharacterUIPrefab)
        {
            _CharacterUIPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "CharacterUI",
                "Prefabs/UI/GameUI/CharacterUI/CharacterUI").GetComponent<CharacterUI>();
        }

        CharacterUI newCharacterUI = _CharacterUIPool.GetRecycledObject() ??
            _CharacterUIPool.RegisterRecyclableObject(Instantiate(_CharacterUIPrefab, transform));

        newCharacterUI.SetOwner(owner);

    }
}
=======
	private CharacterUI _CharacterUIPrefab;

	private ObjectPool<CharacterUI> _CharacterUIPool = new ObjectPool<CharacterUI>();

	public void CreateCharacterWidget(ICharacterUIOwner owner)
	{ 
	
	
	}
}
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
