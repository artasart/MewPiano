using DG.Tweening;
using MEC;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static EasingFunction;
using static Enums;

public class GameUIManager : SingletonManager<GameUIManager>
{
	#region Members

	[NonReorderable] Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
	[NonReorderable] Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();
	[NonReorderable] Dictionary<string, GameObject> splashs = new Dictionary<string, GameObject>();

	[NonReorderable] public List<string> ignorePanels = new List<string>();
	[NonReorderable] public List<string> ignorePopups = new List<string>();

	Stack<UI_Base> openUI = new Stack<UI_Base>();
	Stack<string> openPanels = new Stack<string>();
	Stack<string> openPopups = new Stack<string>();
	Stack<string> openSplashs = new Stack<string>();

	GameObject popup_LastStack;

	GameObject group_MasterCanvas;
	GameObject group_Panel;
	GameObject group_Popup;
	GameObject group_Splash;

	bool isInitialized = false;

	public bool lockBackey = false;

	public Canvas MasterCanvas { get => group_MasterCanvas.GetComponent<Canvas>(); }

	#endregion



	#region Initialize

	private void Awake()
	{
		group_MasterCanvas = GameObject.Find(Define.GO_CANVAS);

		group_Panel = GameObject.Find(nameof(group_Panel));
		group_Popup = GameObject.Find(nameof(group_Popup));
		group_Splash = GameObject.Find(nameof(group_Splash));

		CacheUI(group_Panel, panels);
		CacheUI(group_Popup, popups);
		CacheUI(group_Splash, splashs);

		isInitialized = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameManager.Scene.IsLogoScene() || lockBackey) return;

			Back();
		}
	}

	private void CacheUI(GameObject _parent, Dictionary<string, GameObject> _objects)
	{
		for (int i = 0; i < _parent.transform.childCount; i++)
		{
			var child = _parent.transform.GetChild(i);
			var name = child.name;

			if (_objects.ContainsKey(name))
			{
				DebugManager.Log($"Same Key is registered in {_parent.name}", DebugColor.UI);
				continue;
			}

			child.gameObject.SetActive(true);
			child.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			child.gameObject.SetActive(false);

			_objects[name] = child.gameObject;
		}
	}


	#endregion



	#region Core Methods

	public T FetchPanel<T>() where T : Panel_Base
	{
		if (!panels.ContainsKey(typeof(T).ToString())) return null;

		return panels[typeof(T).ToString()].GetComponent<T>();
	}

	public T StackPanel<T>(bool instant = false, ControlMode mode = ControlMode.UI) where T : Panel_Base
	{
		string panelName = typeof(T).ToString();

		if (openPanels.Contains(panelName)) return default;

		if (panels.ContainsKey(panelName))
		{
			var peekPanel = (openPanels.Count > 0) ? openPanels.Peek() : string.Empty;

			openPanels.Push(panelName);

			panels[panelName].transform.SetAsLastSibling();

			if (instant)
			{
				panels[panelName].SetActive(true);
				panels[panelName].GetComponent<CanvasGroup>().alpha = 1f;
				panels[panelName].GetComponent<CanvasGroup>().blocksRaycasts = true;
				panels[panelName].GetComponent<Panel_Base>().isInstant = true;
			}

			else
			{
				ShowPanel(panels[peekPanel], false);
				ShowPanel(panels[panelName], true);
			}

			switch (mode)
			{
				case ControlMode.UI:
					Cursor.lockState = CursorLockMode.Confined;
					Cursor.visible = true;
					break;
				case ControlMode.Game:
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					break;
				case ControlMode.GameAndUI:
					break;
			}

			panels[panelName].GetComponent<UI_Base>().mode = mode;

			openUI.Push(panels[panelName].GetComponent<UI_Base>());

			DebugManager.Log($"Push: {panelName}", DebugColor.UI);

			return panels[panelName].GetComponent<T>();
		}

		else
		{
			DebugManager.Log($"{panelName} does not exist in this scene.", DebugColor.UI);

			return default;
		}
	}

	public void PopPanel()
	{
		if (openPanels.Count <= 0) return;

		var panelName = openPanels.Pop();
		var peekPanel = (openPanels.Count > 0) ? openPanels.Peek() : string.Empty;

		if (panels[panelName].GetComponent<Panel_Base>().isInstant)
		{
			panels[panelName].SetActive(false);
			panels[panelName].GetComponent<CanvasGroup>().alpha = 0f;
			panels[panelName].GetComponent<CanvasGroup>().blocksRaycasts = false;
			panels[panelName].GetComponent<Panel_Base>().isInstant = false;

			if (!string.IsNullOrEmpty(peekPanel))
			{
				panels[peekPanel].GetComponent<CanvasGroup>().alpha = 1f;
				panels[peekPanel].GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
		}

		else ShowPanel(panels[panelName], false, _callback: () =>
		{
			if (!string.IsNullOrEmpty(peekPanel))
			{
				panels[peekPanel].GetComponent<Panel_Base>().Show(true);
			}
		});

		openUI.Pop();

		RefreshControl();

		DebugManager.Log($"Pop: {panelName}", DebugColor.UI);
	}

	public void PopPanelAll(bool _instant = false)
	{
		while (openPanels.Count > 1)
		{
			var panelName = openPanels.Pop();

			if (panels[panelName].GetComponent<Panel_Base>().isInstant)
			{
				panels[panelName].SetActive(false);
				panels[panelName].GetComponent<CanvasGroup>().alpha = 0f;
				panels[panelName].GetComponent<CanvasGroup>().blocksRaycasts = false;
				panels[panelName].GetComponent<Panel_Base>().isInstant = false;
			}

			else ShowPanel(panels[panelName], false);

			DebugManager.Log($"Pop: {panelName}", DebugColor.UI);
		}

		RefreshControl();
	}

	public bool IsPanelOpen()
	{
		return openPanels.Count > 1;
	}

	public void StartPanel<T>(bool instant = false, ControlMode mode = ControlMode.UI) where T : Panel_Base
	{
		string panelName = typeof(T).ToString();

		ignorePanels.Add(panelName);

		StackPanel<T>(instant, mode);
	}



	public T FetchPopup<T>() where T : Popup_Base
	{
		if (!popups.ContainsKey(typeof(T).ToString())) return null;

		return popups[typeof(T).ToString()].GetComponent<T>();
	}

	public T StackPopup<T>(bool instant = false, ControlMode mode = ControlMode.UI) where T : Popup_Base
	{
		string popupName = typeof(T).Name;

		if (openPopups.Contains(popupName)) return default;

		if (popups.ContainsKey(popupName))
		{
			openPopups.Push(popupName);

			popups[popupName].transform.SetAsLastSibling();

			if (instant)
			{
				popups[popupName].SetActive(true);
				popups[popupName].GetComponent<CanvasGroup>().alpha = 1f;
				popups[popupName].GetComponent<CanvasGroup>().blocksRaycasts = true;
				popups[popupName].GetComponent<Popup_Base>().isInstant = true;
				popups[popupName].transform.Search(Define.GROUP_MODAL).localScale = Vector3.one;
			}
			else
			{
				ShowPopup(popups[popupName], popups[popupName].GetComponent<Popup_Base>().isInstant);
			}

			openUI.Push(popups[popupName].GetComponent<UI_Base>());

			popups[popupName].GetComponent<UI_Base>().mode = mode;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;

			DebugManager.Log($"Push: {popupName}", DebugColor.UI);

			return popups[popupName].GetComponent<T>();
		}
		else
		{
			DebugManager.Log($"{popupName} does not exist in this scene.", DebugColor.UI);
			return default;
		}
	}

	public void LastPopup<T>()
	{
		string popupName = typeof(T).Name;

		if (!popups.ContainsKey(popupName)) return;

		popup_LastStack = popups[popupName];
	}

	public void PopPopup(bool instant = false)
	{
		if (openPopups.Count <= 0) return;

		var popupName = openPopups.Pop();

		if (instant)
		{
			popups[popupName].SetActive(false);
			popups[popupName].GetComponent<CanvasGroup>().alpha = 0f;
			popups[popupName].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}

		else HidePopup(popups[popupName], popups[popupName].GetComponent<Popup_Base>().isInstant);

		openUI.Pop();

		RefreshControl();

		DebugManager.Log($"Pop: {popupName}", DebugColor.UI);
	}

	public void PopPopupAll(bool _instant = false)
	{
		while (openPopups.Count > 0)
		{
			var popupName = openPopups.Pop();

			if (popups[popupName].GetComponent<Popup_Base>().isInstant)
			{
				popups[popupName].SetActive(false);
				popups[popupName].GetComponent<CanvasGroup>().alpha = 0f;
				popups[popupName].GetComponent<CanvasGroup>().blocksRaycasts = false;
			}

			else HidePopup(popups[popupName], popups[popupName].GetComponent<Popup_Base>().isInstant);

			RefreshControl();

			DebugManager.Log($"Pop: {popupName}", DebugColor.UI);
		}

		RefreshControl();
	}

	public T StartPopup<T>(bool isInstant = false) where T : Popup_Base
	{
		string popupName = typeof(T).Name;

		if (!ignorePopups.Contains(popupName))
		{
			ignorePopups.Add(popupName);

			T popup = StackPopup<T>(isInstant);

			if (popup == null)
			{
				DebugManager.Log($"{popupName} could not be started.", DebugColor.UI);
			}

			return popup;
		}
		else
		{
			DebugManager.Log($"{popupName} is already in the ignore list.", DebugColor.UI);
			return null;
		}
	}

	public void PopIgnorePopup(bool instant = false)
	{
		if (ignorePopups.Count <= 0) return;

		var popupName = openPopups.Pop();

		if (instant)
		{
			popups[popupName].SetActive(false);
			popups[popupName].GetComponent<CanvasGroup>().alpha = 0f;
			popups[popupName].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}

		else
		{
			HidePopup(popups[popupName], popups[popupName].GetComponent<Popup_Base>().isInstant);
		}

		ignorePopups.Remove(popupName);

		DebugManager.Log($"Pop: {popupName}", DebugColor.UI);
	}



	public bool IsPopupOpen()
	{
		return openPopups.Count > 0;
	}



	public T FetchSplash<T>() where T : Splash_Base
	{
		if (!splashs.ContainsKey(typeof(T).ToString())) return null;

		return splashs[typeof(T).ToString()].GetComponent<T>();
	}

	public T StackSplash<T>(bool instant = false) where T : Splash_Base
	{
		string splashName = typeof(T).Name;

		if (splashs.ContainsKey(splashName))
		{
			openSplashs.Push(splashName);

			splashs[splashName].transform.SetAsLastSibling();

			if (instant)
			{
				splashs[splashName].SetActive(true);
				splashs[splashName].GetComponent<CanvasGroup>().alpha = 1f;
				splashs[splashName].GetComponent<CanvasGroup>().blocksRaycasts = true;
				splashs[splashName].GetComponent<Splash_Base>().isInstant = true;
			}
			else
			{
				Show(splashs[splashName], true);
			}

			openUI.Push(splashs[splashName].GetComponent<UI_Base>());

			splashs[splashName].GetComponent<UI_Base>().mode = ControlMode.UI;

			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;

			DebugManager.Log($"Push: {splashName}", DebugColor.UI);

			return splashs[splashName].GetComponent<T>();
		}
		else
		{
			DebugManager.Log($"{splashName} does not exist in this scene.", DebugColor.UI);
			return default;
		}
	}

	public void PopSplash()
	{
		if (openSplashs.Count <= 0) return;

		var splashName = openSplashs.Pop();

		if (splashs[splashName].GetComponent<Splash_Base>().isInstant)
		{
			splashs[splashName].SetActive(false);
			splashs[splashName].GetComponent<CanvasGroup>().alpha = 0f;
			splashs[splashName].GetComponent<CanvasGroup>().blocksRaycasts = false;
			splashs[splashName].GetComponent<Splash_Base>().isInstant = false;
		}

		else Show(splashs[splashName], false);

		openUI.Pop();
		RefreshControl();

		DebugManager.Log($"Pop: {splashName}", DebugColor.UI);
	}


	#endregion



	#region Basic Methods

	public void Back()
	{
		if (openSplashs.Count > 0)
		{
			PopSplash();

			return;
		}

		if (openPopups.Count > 0)
		{
			if (ignorePopups.Contains(openPopups.Peek()))
			{
				DebugManager.ClearLog("This popup is ignore popup.", DebugColor.UI);

				return;
			}

			else
			{
				var isInstant = popups[openPopups.Peek()].GetComponent<Popup_Base>().isInstant;

				PopPopup(isInstant);

				GameManager.Sound.PlaySound(Define.SOUND_CLOSE);
			}
		}

		else if (openPanels.Count > 0)
		{
			if (ignorePanels.Contains(openPanels.Peek()))
			{
				if (openPopups.Count <= 0)
				{
					var popupName = popup_LastStack.name;

					if (popups.ContainsKey(popupName))
					{
						GameManager.Sound.PlaySound(Define.SOUND_OPEN);

						openPopups.Push(popup_LastStack.name);

						popups[popup_LastStack.name].GetComponent<UI_Base>().mode = ControlMode.UI;

						openUI.Push(popups[popup_LastStack.name].GetComponent<UI_Base>());

						popup_LastStack.transform.SetAsLastSibling();

						ShowPopup(popup_LastStack, popups[popup_LastStack.name].GetComponent<Popup_Base>().isInstant);
												
						Cursor.lockState = CursorLockMode.Confined;
						Cursor.visible = true;

						DebugManager.Log($"Push: {popupName}", DebugColor.UI);
					}

					else { DebugManager.Log($"No Popup name {popupName}", DebugColor.UI); }
				}

				return;
			}

			PopPanel();

			GameManager.Sound.PlaySound(Define.SOUND_CLOSE);
		}
	}

	public void Restart()
	{
		if (!isInitialized) return;

		panels.Clear();
		popups.Clear();
		openPanels.Clear();
		openPopups.Clear();
		ignorePanels.Clear();
		splashs.Clear();

		group_MasterCanvas = GameObject.Find(Define.GO_CANVAS);

		group_Panel = GameObject.Find(nameof(group_Panel));
		group_Popup = GameObject.Find(nameof(group_Popup));
		group_Splash = GameObject.Find(nameof(group_Splash));

		popup_LastStack = null;

		CacheUI(group_Panel, panels);
		CacheUI(group_Popup, popups);
		CacheUI(group_Splash, splashs);
	}



	private void ShowPanel(GameObject _gameObject, bool _isShow, Action _callback = null)
	{
		Util.RunCoroutine(Co_Show(_gameObject, _isShow, 1f, _callback), nameof(Co_Show) + _gameObject.GetHashCode(), CoroutineTag.UI);
	}

	private void ShowPopup(GameObject gameObject, bool isInstant)
	{
		gameObject.SetActive(true);

		RectTransform rectTransform = gameObject.transform.Search("group_Modal").GetComponent<RectTransform>();
		CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

		if(isInstant)
		{
			rectTransform.localScale = Vector3.one;
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
			return;
		}

		rectTransform.localScale = Vector3.zero;
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;

		rectTransform.localScale = Vector3.one * .75f;

		gameObject.GetComponent<Popup_Base>().OpenSequence();
		rectTransform.DOScale(Vector3.one * 1.1f, .2f).OnComplete(() => { rectTransform.DOScale(Vector3.one, .2f); });
		canvasGroup.DOFade(1f, .25f).OnComplete(() => { canvasGroup.blocksRaycasts = true; });
	}

	private void HidePopup(GameObject gameObject, bool isInstant)
	{
		RectTransform rectTransform = gameObject.transform.Search("group_Modal").GetComponent<RectTransform>();
		CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
		canvasGroup.blocksRaycasts = false;

		if (isInstant)
		{
			rectTransform.localScale = Vector3.one;
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			gameObject.SetActive(false);
			return;
		}

		gameObject.GetComponent<Popup_Base>().CloseSequence();
		rectTransform.DOScale(Vector3.one * .75f, .15f).OnComplete(() => { gameObject.SetActive(false); });
		canvasGroup.DOFade(0f, .5f);
	}

	private void Show(GameObject _gameObject, bool _isShow, float _lerpspeed = 1f, Action _callback = null)
	{
		Util.RunCoroutine(Co_Show(_gameObject, _isShow, _lerpspeed, _callback), nameof(Co_Show) + _gameObject.GetHashCode(), CoroutineTag.UI);
	}

	private IEnumerator<float> Co_Show(GameObject _gameObject, bool _isShow, float _lerpspeed = 1f, Action callback = null)
	{
		var canvasGroup = _gameObject.GetComponent<CanvasGroup>();
		var targetAlpha = _isShow ? 1f : 0f;
		var lerpvalue = 0f;
		var lerpspeed = _lerpspeed;

		if (canvasGroup == null) yield break;

		if (!_isShow) canvasGroup.blocksRaycasts = false;
		else _gameObject.SetActive(true);

		while (canvasGroup != null && Mathf.Abs(canvasGroup.alpha - targetAlpha) >= 0.001f)
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, lerpvalue += lerpspeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;
		}

		if (canvasGroup == null) yield break;

		canvasGroup.alpha = targetAlpha;

		if (_isShow) canvasGroup.blocksRaycasts = true;
		else _gameObject.SetActive(false);

		callback?.Invoke();
	}

	private void RefreshControl()
	{
		switch (openUI.Peek().mode)
		{
			case ControlMode.Game:
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				break;
			case ControlMode.UI:
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				break;
		}
	}

	#endregion
}