using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;

public class GameWebManager : SingletonManager<GameWebManager>
{
	private IEnumerator<float> Co_GetRequest<T>(string url, Action<T> callback = null, bool useDim = false)
	{
		if (useDim) GameManager.Scene.Dim(true);

		using (UnityWebRequest request = UnityWebRequest.Get(url))
		{
			yield return Timing.WaitUntilDone(request.SendWebRequest());

			if (request.result != UnityWebRequest.Result.Success)
			{
				var result = JsonUtility.FromJson<DefaultRes>(request.downloadHandler.text);
				DebugManager.Log(result != null ? result.message : "Network Error");
			}
			else
			{
				callback?.Invoke(JsonUtility.FromJson<T>(request.downloadHandler.text));
			}
		}

		if (useDim) GameManager.Scene.Dim(false);
	}

	private IEnumerator<float> Co_PutRequest<T>(string url, string jsonData, Action<T> callback = null, Action<T> error = null, bool useDim = false)
	{
		if (useDim) GameManager.Scene.Dim(true);

		byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonData);

		using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
		{
			request.uploadHandler = new UploadHandlerRaw(body);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			yield return Timing.WaitUntilDone(request.SendWebRequest());

			T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
			if (request.result != UnityWebRequest.Result.Success) error?.Invoke(result);
			else callback?.Invoke(result);
		}

		if (useDim) GameManager.Scene.Dim(false);
	}

	private IEnumerator<float> Co_DeleteRequest<T>(string url, Action<T> callback = null, Action<T> error = null, bool useDim = false)
	{
		if (useDim) GameManager.Scene.Dim(true);

		using (UnityWebRequest request = UnityWebRequest.Delete(url))
		{
			yield return Timing.WaitUntilDone(request.SendWebRequest());

			T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
			if (request.result != UnityWebRequest.Result.Success) error?.Invoke(result);
			else callback?.Invoke(result);
		}

		if (useDim) GameManager.Scene.Dim(false);
	}

}
