using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : ScreenDrawableUI, IObjectPoolable
{
<<<<<<< HEAD
    [Header("최대 표시 거리")]
    [Tooltip("카메라와의 거리가 해당 값보다 크다면 보이지 않습니다.")]
    [SerializeField] private float _MaxVisibleDistance = 30.0f;

    [Header("Offset")]
    [SerializeField] private Vector3 _DrawOffset;

    // CharacterUI 를 소융하는 객체
    private ICharacterUIOwner _Owner;

    // UI 요소들의 부모 오브젝트
    private GameObject _Content;

    public bool canRecyclable { get ; set; }
    public Action onRecycleStartDelegate { get; set; }

    private void Awake()
    {
        _Content = transform.Find("Content").gameObject;
    }

    protected virtual void Update()
    {
        base.Update();

        drawPosition = _Owner.characterUIPosition + _DrawOffset;

        HideByDistance();
    }

    // 거리에 따라 UI 를 숨깁니다.
    private void HideByDistance()
    {
        // 카메라와, UI를 소유하는 오브젝트 사이의 거리가 _MaxVisibleDistnace 이하 인가?
        bool visible = Vector3.Distance(
            camera.transform.position, _Owner.transform.position) <= _MaxVisibleDistance;

        _Content.SetActive(visible);

    }

    public void SetOwner(ICharacterUIOwner owner)
    {
        _Owner = owner;
    }
=======
	public bool canRecyclable { get; set; }
	public Action onRecycleStartDelegate { get; set; }
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
}
