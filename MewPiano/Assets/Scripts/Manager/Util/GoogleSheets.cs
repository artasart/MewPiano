using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using MEC;
using static Enums;
public static class GoogleSheets
{
	public static Queue<Action> actionQueue = new Queue<Action>();
	public static Queue<string> messages = new Queue<string>();

	public static bool IsLoading { get; private set; } = true;
	public static bool IsSaved { get; private set; } = true;

	public static void AddJob(string message, Action action)
	{
		messages.Enqueue(message);
		actionQueue.Enqueue(action);
	}

	public static void GetDataAll(Action callback = null)
	{
		if (actionQueue.Count <= 0)
		{
			DebugManager.Log("No Job.", DebugColor.Data);

			return;
		};

		Util.RunCoroutine(Co_GetMasterData(callback), nameof(Co_GetMasterData));
	}

	private static IEnumerator<float> Co_GetMasterData(Action callback = null)
	{
		while (actionQueue.Count > 0)
		{
			var message = messages.Dequeue();

			DebugManager.Log(message, DebugColor.Data);

			actionQueue.Dequeue()?.Invoke();

			yield return Timing.WaitUntilFalse(() => IsLoading);

			yield return Timing.WaitUntilTrue(() => GameManager.Data.isSaved);
		}

		callback?.Invoke();
	}

	public static void GetData(long sheetID, Action<string> callback = null)
	{
		//string sheetURL = GetTSVAddress(Url.SHEET_URL, sheetID);

		//Util.RunCoroutine(Co_GetSheetData(sheetURL, callback), sheetURL, CoroutineTag.Web);
	}

	private static IEnumerator<float> Co_GetSheetData(string sheetURL, Action<string> callback)
	{
		IsLoading = true;

		using (UnityWebRequest www = UnityWebRequest.Get(sheetURL))
		{
			yield return Timing.WaitUntilDone(www.SendWebRequest());

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"Failed to load data: {www.error}");

				yield break;
			}

			string data = www.downloadHandler.text;

			callback?.Invoke(data);

			IsLoading = false;
		}
	}

	public static string GetTSVAddress(string address, long sheetID)
	{
		return $"{address}/export?format=tsv&gid={sheetID}";
	}
}