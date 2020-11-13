using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenDrawableUI : MonoBehaviour
{
<<<<<<< HEAD
    public new Camera camera { get; private set; }

    public Vector3 drawPosition { get; protected set; }

    public RectTransform rectTransform => transform as RectTransform;

    protected virtual void Start()
    {
        camera = PlayerManager.Instance.playerCharacter.springArm.camera;
    }

    protected virtual void Update()
    {
        DrawUI();
    }

    private void DrawUI()
    {
        Vector3 screenPosition = camera.WorldToViewportPoint(drawPosition);

        screenPosition.x *= (Screen.width / GameStatics.screenRatio);
        screenPosition.y *= (Screen.height / GameStatics.screenRatio);

        rectTransform.anchoredPosition = screenPosition;
    }



=======
	public new Camera camera { get; private set; }

	public Vector3 drawPosition { get; protected set; }

	public RectTransform rectTransform => transform as RectTransform;

	protected virtual void Start()
	{
		camera = PlayerManager.Instance.playerCharacter.springArm.camera;
	}

	protected virtual void Update()
	{
		DrawUI();
	}

	private void DrawUI()
	{
		Vector3 screenPosition = camera.WorldToViewportPoint(drawPosition);

		screenPosition.x *= (Screen.width / GameStatics.screenRatio);
		screenPosition.y *= (Screen.height / GameStatics.screenRatio);

		rectTransform.anchoredPosition = screenPosition;
	}
	
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
}
