
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

// 애셋 경로 문자열을 리소스 로드시 사용할 수 있는 경로로 변경해주는 창
public sealed class PathFixer : EditorWindow
{
	/// - EditorWindow : 에디터에서 사용할 수 있는 창을 나타냅니다.

	// 기존 애셋 경로를 저장할 변수
	public string m_Path = "Input Path...";

	// 변환된 경로를 저장할 변수
	public string m_Convert;


	[MenuItem("Window/CustomUtil/PathFixer")]
	private static void Init()
	{
		// PathFixer 창 객체를 얻습니다.
		PathFixer window = GetWindow(typeof(PathFixer)) as PathFixer;

		// 창의 최소 크기를 설정합니다.
		window.minSize = new UnityEngine.Vector2(20, 100);

		// 생성된 창을 표시합니다.
		window.Show();
	}

	// GUI 이벤트를 다루고 렌더링할 때 사용되는 메서드
	private void OnGUI()
	{
		// 라벨
		GUILayout.Label("Base Path", EditorStyles.boldLabel);
		/// - GUILayout : 자동 배치 기능을 사용하는 Unity GUI 를 위한 클래스입니다.
		/// - EditorStyles : Editor GUI 에 사용되는 GUI 스타일입니다.

		// 경로 입력 칸
		m_Path = EditorGUILayout.TextField("Input Path", m_Path);

		// 경로 변환 버튼
		/// - 버튼이 눌렸을 경우
		if (GUILayout.Button("Convert"))
		{
			// 문자열을 변환시킵니다.
			m_Convert = ConvertPath(m_Path);
		}

		EditorGUILayout.TextField("Result Path", m_Convert);
	}

	// 애셋 로드에 사용할 수 있는 경로로 변환해 반환하는 메서드
	/// - original : 원본 경로
	/// - return : 변환된 경로
	private string ConvertPath(string original)
	{
		int pathStartIndex = original.IndexOf("Resources/") + ("Resources/".Length);

		int dotIndex = original.IndexOf(".");

		return $"\"{original.Substring(pathStartIndex, dotIndex - pathStartIndex)}\"";
	}
}

#endif