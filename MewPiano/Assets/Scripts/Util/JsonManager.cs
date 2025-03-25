using UnityEngine;
using System.IO;

public static class JsonManager<T>
{
	public static void SaveData(T data, string fileName)
	{
		string folderPath = Application.persistentDataPath + "/GameData";

		string filePath = Path.Combine(folderPath, fileName + ".json");

		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}

		string json = JsonUtility.ToJson(data);

		File.WriteAllText(filePath, json);

		DebugManager.Log($"{fileName} saved safely!", DebugColor.Data);
	}

	public static T LoadData(string fileName)
	{
		string filePath = Path.Combine(Application.persistentDataPath, "GameData", fileName + ".json");

		// 파일이 존재하는지 확인합니다.
		if (!File.Exists(filePath))
		{
			Debug.LogWarning("File does not exist: " + fileName);
			return default(T);
		}

		// 파일에서 데이터를 읽어옵니다.
		string json = File.ReadAllText(filePath);

		// JSON 데이터를 객체로 역직렬화합니다.
		return JsonUtility.FromJson<T>(json);
	}
}