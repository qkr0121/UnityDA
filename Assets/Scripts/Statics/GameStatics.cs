

using UnityEngine;

public static class GameStatics
{
<<<<<<< HEAD
	// 캔버스 언본 사이즈에 대한 읽기 전용 프로퍼티
=======
	// 캔버스 원본 사이즈에 대한 읽기 전용 프로퍼티
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
	public static (float width, float height) screenSize => (1600.0f, 900.0f);

	// 화면 비율에 대한 읽기 전용 프로퍼티
	public static float screenRatio => Screen.width / screenSize.width;

	// 화면 중간 좌표에 대한 읽기 전용 프로퍼티
	public static Vector2 screenCenterPosition =>
		new Vector2(screenSize.width, screenSize.height) * 0.5f;
<<<<<<< HEAD
}
=======

}
>>>>>>> 9655e7a1d2eec00892d6511064e389e7cc734bd0
