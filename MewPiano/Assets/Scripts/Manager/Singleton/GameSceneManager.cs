using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class GameSceneManager : SingletonManager<GameSceneManager>
{
	#region Members

	GameObject go_MasterCanvas;

	public GameObject go_Fade { get; set; }
	public GameObject go_Dim { get; set; }
	public GameObject go_Toast { get; set; }

	bool isFadeDone = false;
	bool isDimDone = false;

	bool isFade = false;
	bool isDim = false;

	public SceneName current;

	#endregion



	#region Initialize

	private void Awake()
	{
		CacheTransUI();
	}

	private void CacheTransUI()
	{
		go_MasterCanvas = Instantiate(Resources.Load<GameObject>(Define.PATH_UI + Define.GO_CANVASMASTER), Vector3.zero, Quaternion.identity, this.transform);
		go_MasterCanvas.name = Define.GO_CANVASMASTER;

		go_Fade = CreateTransUI(Resources.Load<GameObject>(Define.PATH_UI + nameof(go_Fade)));
		go_Dim = CreateTransUI(Resources.Load<GameObject>(Define.PATH_UI + nameof(go_Dim)));
		go_Toast = CreateTransUI(Resources.Load<GameObject>(Define.PATH_UI + nameof(go_Toast)));
	}

	private GameObject CreateTransUI(GameObject prefab)
	{
		var element = Instantiate(prefab, Vector3.zero, Quaternion.identity, go_MasterCanvas.transform);
		var rectTransform = element.GetComponent<RectTransform>();
		var canvasGroup = element.GetComponent<CanvasGroup>();

		rectTransform.localPosition = Vector3.zero;
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;

		element.SetActive(false);

		return element;
	}

	#endregion



	#region Core Methods

	public void LoadScene(SceneName sceneName, bool isAsync = true, float fadeSpeed = .5f, Action action = null) => Util.RunCoroutine(Co_LoadScene(sceneName, isAsync, fadeSpeed, action), nameof(Co_LoadScene));

	private IEnumerator<float> Co_LoadScene(SceneName sceneName, bool isAsync = true, float fadeSpeed = 1f, Action action = null)
	{
		Fade(true, fadeSpeed);

		yield return Timing.WaitUntilTrue(() => isFadeDone);

		string name = GetSceneName(sceneName);

		if (isAsync)
		{
			SceneManager.LoadSceneAsync(name);
		}

		else
		{
			SceneManager.LoadScene(name);
		}

		yield return Timing.WaitUntilTrue(() => IsSceneLoaded(name));

		current = sceneName;

		action?.Invoke();

		DebugManager.Log($"{sceneName} is loaded.", DebugColor.Scene);
	}

	#endregion



	#region Basic Methods

	public void UnloadSceneAsync(string _sceneName)
	{
		SceneManager.UnloadSceneAsync(_sceneName);

		DebugManager.Log($"{_sceneName} is unloaded async.", DebugColor.Scene);
	}

	public bool IsSceneLoaded(string _sceneName)
	{
		return SceneManager.GetSceneByName(_sceneName).isLoaded;
	}

	public string GetCurrentSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public int GetCurrentSceneBuildIndex()
	{
		return SceneManager.GetActiveScene().buildIndex;
	}

	public string GetSceneNameByBuildIndex(int _buildIndex)
	{
		Scene scene = SceneManager.GetSceneByBuildIndex(_buildIndex);

		return scene.name;
	}

	public string GetSceneName(SceneName _sceneName)
	{
		string sceneName = string.Empty;

		switch (_sceneName)
		{
			case SceneName.Logo:
				sceneName = "01_" + _sceneName.ToString();
				break;
			case SceneName.Title:
				sceneName = "02_" + _sceneName.ToString();
				break;
			case SceneName.Main:
				sceneName = "03_" + _sceneName.ToString();
				break;
			case SceneName.Game:
				sceneName = "04_" + _sceneName.ToString();
				break;
		}

		return sceneName;
	}

	#endregion



	#region Utils

	public void Fade(bool isFade, float fadeSpeed = .75f)
	{
		if (this.isFade == isFade) return;

		Util.RunCoroutine(Co_FadeTransition(go_Fade, isFade, fadeSpeed), nameof(Co_FadeTransition));

		DebugManager.Log($"Fade : {isFade}", DebugColor.UI);
	}

	private IEnumerator<float> Co_FadeTransition(GameObject meta, bool isFade, float lerpSpeed)
	{
		isFadeDone = false;

		meta.transform.SetAsLastSibling();
		meta.SetActive(true);

		var handle_Show = Timing.RunCoroutine(Co_Show(meta, isFade, lerpSpeed), nameof(Co_Show) + meta.GetHashCode());

		yield return Timing.WaitUntilDone(handle_Show);

		if (!isFade)
		{
			meta.SetActive(false);
		}

		isFadeDone = true;
		this.isFade = isFade;
	}

	public void Dim(bool isDim, float dimSpeed = 1f)
	{
		Timing.KillCoroutines(go_Dim.GetHashCode());

		Util.RunCoroutine(Co_DimTransition(go_Dim, isDim, dimSpeed), nameof(Co_DimTransition));

		DebugManager.Log($"Dim : {isDim}", DebugColor.UI);
	}

	private IEnumerator<float> Co_DimTransition(GameObject meta, bool isDim, float lerpSpeed)
	{
		isDimDone = false;

		meta.transform.SetAsLastSibling();
		meta.SetActive(true);

		var handle_Show = Timing.RunCoroutine(Co_Show(meta, isDim, lerpSpeed), meta.GetHashCode());

		yield return Timing.WaitUntilDone(handle_Show);

		if (!isDim)
		{
			meta.SetActive(false);
		}

		isDimDone = true;
	}

	public bool IsFaded()
	{
		return isFadeDone;
	}

	private IEnumerator<float> Co_Show(GameObject gameObject, bool isShow, float lerpSpeed = 1f)
	{
		var canvasGroup = gameObject.GetComponent<CanvasGroup>();
		var targetAlpha = isShow ? 1f : 0f;
		var lerpvalue = 0f;
		var lerpspeed = lerpSpeed;

		if (!isShow) canvasGroup.blocksRaycasts = true;
		else gameObject.SetActive(true);

		while (Mathf.Abs(canvasGroup.alpha - targetAlpha) >= 0.001f)
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, lerpvalue += lerpspeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;
		}

		canvasGroup.alpha = targetAlpha;

		if (isShow) canvasGroup.blocksRaycasts = false;
		else gameObject.SetActive(false);
	}

	public void FadeInstant(bool enable)
	{
		isFade = enable;

		go_Fade.SetActive(enable);
		go_Fade.GetComponent<CanvasGroup>().alpha = enable ? 1f : 0f;
		go_Fade.GetComponent<CanvasGroup>().blocksRaycasts = enable;

		isFadeDone = true;
	}



	public void ShowToast(string message)
	{
		CancelInvoke(nameof(HideToast));

		go_Toast.gameObject.SetActive(true);
		go_Toast.GetComponent<CanvasGroup>().alpha = 1;
		go_Toast.GetComponent<CanvasGroup>().blocksRaycasts = true;
		go_Toast.GetComponent<Toast_Default>().Show(message);
	}

	public void HideToast()
	{
		DebugManager.Log("Hide Toast.");

		go_Toast.GetComponent<Toast_Default>().Hide();
	}

	public void ShowAndHideToast(string message)
	{
		CancelInvoke(nameof(HideToast));

		ShowToast($"{message}");

		Invoke(nameof(HideToast), 2f);
	}



	public bool IsLogoScene()
	{
		return GetCurrentSceneName() == "01_" + SceneName.Logo.ToString();
	}

	#endregion
}
