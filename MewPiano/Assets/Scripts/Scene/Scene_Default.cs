using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class Scene_Default : MonoBehaviour
{
	public GameObject gameUI { get; private set; }
	public GameObject gameWorld { get; private set; }

	protected virtual void OnDestroy()
	{
		Timing.KillCoroutines((int)CoroutineTag.UI);
		Timing.KillCoroutines((int)CoroutineTag.Content);
	}

	protected virtual void Awake()
	{
		FindFirstObjectByType<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
		Screen.SetResolution(Screen.width, Screen.height, true);

		Application.targetFrameRate = 60;

		var gameManager = GameObject.Find(Define.GAMEMANAGER) ?? Util.Instantiate(Define.PATH_MANAGER + Define.GAMEMANAGER, Vector3.zero, Quaternion.identity);
		gameManager.name = Define.GAMEMANAGER;

		gameUI = GameObject.Find(Define.GAMEUI);
		gameWorld = GameObject.Find(Define.GAMEWORLD);
	}

	protected virtual void Start()
	{
		GameManager.UI.Restart();

		GameManager.UI.lockBackey = false;

		Util.RunCoroutine(Co_Start(), nameof(Co_Start));
	}

	IEnumerator<float> Co_Start()
	{
		GameManager.Scene.FadeInstant(true);

		yield return Timing.WaitForSeconds(.5f);

		GameManager.Scene.Fade(false);
	}

	protected virtual void Initialize()
	{

	}
}