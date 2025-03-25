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

		// ������ �����ϴ��� Ȯ���մϴ�.
		if (!File.Exists(filePath))
		{
			Debug.LogWarning("File does not exist: " + fileName);
			return default(T);
		}

		// ���Ͽ��� �����͸� �о�ɴϴ�.
		string json = File.ReadAllText(filePath);

		// JSON �����͸� ��ü�� ������ȭ�մϴ�.
		return JsonUtility.FromJson<T>(json);
	}
}