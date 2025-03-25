using MEC;
using UnityEngine;
using static Enums;

public static class GameManager
{
	public static GameSceneManager Scene { get { return GameSceneManager.Instance; } }
	public static GameSoundManager Sound { get { return GameSoundManager.Instance; } }
	public static GameUIManager UI { get { return GameUIManager.Instance; } }
	public static GameDataManager Data { get { return GameDataManager.Instance; } }
}

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			Application.targetFrameRate = 60;

			if (instance == null)
			{
				instance = FindFirstObjectByType<T>() ?? new GameObject(typeof(T).Name).AddComponent<T>();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	public virtual void OnDestroy()
	{
		Timing.KillCoroutines((int)CoroutineTag.Web);
		Timing.KillCoroutines((int)CoroutineTag.UI);
		Timing.KillCoroutines((int)CoroutineTag.Content);
	}
}