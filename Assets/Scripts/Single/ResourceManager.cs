using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;

// 게임 실행중 로드한 리소스들을 관리하는 클래스입니다.
public sealed class ResourceManager : ManagerClassBase<ResourceManager>
{
	// Json 파일을 저장하기 위한 경로를 저장할 변수
	private string _JsonFolderPath;

	// 로드한 리소스들을 저장합니다.
	private Dictionary<string, Object> _LoadedResources = new Dictionary<string, Object>();

	public Object this[string resourceName] => _LoadedResources[resourceName];


	public override void InitializeManagerClass() 
	{
		_JsonFolderPath = $"{Application.dataPath}/Resources/Json/";
	}

	// 특정한 형식으로 리소스를 로드하여 반환합니다.
	public T LoadResource<T>(string resouceName, string resourcePath = null) where T : class
	{
		// 만약 이미 resouceName 으로 등록되어있는 요소가 존재한다면
		if (_LoadedResources.ContainsKey(resouceName))
			return this[resouceName] as T;

		// 만약 등록되어있지 않다면
		else
		{
			// 리소스를 로드합니다.
			Object loadedResource = Resources.Load(resourcePath);

			// 만약 리소스가 로드되었다면
			if (loadedResource)
			{
				_LoadedResources.Add(resouceName, loadedResource);
				return loadedResource as T;
			}
			// 만약 리소스가 로드되지 않았다면
			else
			{
				// 에디터의 경우에만 로그를 띄웁니다.
#if UNITY_EDITOR
				Debug.LogError($"{resouceName} is not loaded! (path : {resourcePath})");
#endif

				// 정상적으로 로드되지 않았으므로 null 을 반환합니다.
				return null;
			}
		}
	}

	public T LoadJson<T>(string filePath, out bool fileNotFound) where T : struct
	{
		string jsonData = null;

		try
		{
			jsonData = File.ReadAllText(_JsonFolderPath + filePath);
		}
		catch (DirectoryNotFoundException)
		{
			fileNotFound = true;
			return new T();
		}
		catch (FileNotFoundException)
		{
			fileNotFound = true;
			return new T();
		}

		fileNotFound = false;
		return JsonUtility.FromJson<T>(jsonData);
	}

	// json 파일을 저장합니다.
	public void SaveJson<T>(T data, string folderPath, string fileName) where T : struct
	{
		Directory.CreateDirectory(_JsonFolderPath + folderPath);

		string jsonString = JsonUtility.ToJson(data, true);
		File.WriteAllText($"{_JsonFolderPath}{folderPath}/{fileName}.json", jsonString);
	}
}
