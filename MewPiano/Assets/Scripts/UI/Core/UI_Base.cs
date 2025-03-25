using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Enums;

public class UI_Base : MonoBehaviour
{
	Dictionary<string, GameObject> childUI = new Dictionary<string, GameObject>();
	[HideInInspector] public bool isInstant = false;
	[HideInInspector] public ControlMode mode;

	#region Initialize

	protected virtual void Awake()
	{
		FindAllChildUI();
	}

	private void FindAllChildUI() => SearchUI(this.transform);

	private void SearchUI(Transform parent)
	{
		Stack<Transform> stack = new Stack<Transform>();
		stack.Push(parent);

		while (stack.Count > 0)
		{
			Transform current = stack.Pop();

			foreach (Transform child in current)
			{
				var tab = child.GetComponent<Tab_Base>();
				var item = child.GetComponent<Item_Base>();

				if (tab != null || item != null)
				{
					if (!childUI.ContainsKey(child.name))
					{
						childUI[child.name] = child.gameObject;
					}
				}
				else
				{
					if (!childUI.ContainsKey(child.name))
					{
						childUI[child.name] = child.gameObject;
					}

					stack.Push(child);
				}
			}
		}
	}

	#endregion



	#region Basic Methods

	public TMP_Text GetUI_TMPText(string hierarchyName, string message = "")
	{
		if (childUI.ContainsKey(hierarchyName))
		{
			var txtmp = childUI[hierarchyName].GetComponent<TMP_Text>();

			if (!string.IsNullOrEmpty(message)) txtmp.text = message;

			return txtmp;
		}

		else { DebugManager.Log($"WARNING : {hierarchyName} is not in this hierarchy."); return null; }
	}

	public virtual Button GetUI_Button(string hierarchyName, Action action = null, Action sound = null, bool useSound = true, bool useAnimation = false)
	{
		if (childUI.ContainsKey(hierarchyName))
		{
			var button = childUI[hierarchyName].GetComponent<Button>();

			if (sound == null)
			{
				if (useSound)
				{
					button.onClick.AddListener(OpenSound);
				}
			}

			else button.onClick.AddListener(() => sound?.Invoke());

			button.onClick.AddListener(() => action?.Invoke());

			if (useAnimation) button.UseAnimation();

			return button;
		}

		else { DebugManager.Log($"WARNING : {hierarchyName} is not in this hierarchy."); return null; }
	}

	public Image GetUI_Image(string _hierarchyName, Sprite _sprite = null, bool _default = true)
	{
		if (childUI.ContainsKey(_hierarchyName))
		{
			var image = childUI[_hierarchyName].GetComponent<Image>();

			if (!_default) image.sprite = _sprite;

			return image;
		}

		else { DebugManager.Log($"WARNING : {_hierarchyName} is not in this hierarchy."); return null; }
	}

	public Toggle GetUI_Toggle(string _hierarchyName, bool _isToggleOn = false, UnityAction<bool> onValueChanged = null)
	{
		if (childUI.ContainsKey(_hierarchyName))
		{
			var toggle = childUI[_hierarchyName].GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(onValueChanged);
			toggle.isOn = _isToggleOn;

			return toggle;
		}

		else { DebugManager.Log($"WARNING : {_hierarchyName} is not in this hierarchy."); return null; }
	}

	public Slider GetUI_Slider(string _hierarchyName, Action<float> _action = null)
	{
		if (childUI.ContainsKey(_hierarchyName))
		{
			var slider = childUI[_hierarchyName].GetComponent<Slider>();

			slider.onValueChanged.AddListener((value) => _action?.Invoke(value));

			return slider;
		}

		else { DebugManager.Log($"WARNING : {_hierarchyName} is not in this hierarchy."); return null; }
	}

	public TMP_InputField GetUI_TMPInputField(string _hierarchyName, Action<string> _action = null)
	{
		if (childUI.ContainsKey(_hierarchyName))
		{
			var inputField = childUI[_hierarchyName].GetComponent<TMP_InputField>();
			var placeHolder = inputField.placeholder.GetComponent<TextMeshProUGUI>().text;

			inputField.onValueChanged.AddListener((value) => _action?.Invoke(value));

			inputField.onSelect.AddListener((value) =>
			{
				if (placeHolder != string.Empty)
				{
					inputField.placeholder.GetComponent<TextMeshProUGUI>().text = string.Empty;
				}
			});

			inputField.onDeselect.AddListener((value) =>
			{
				if (inputField.text == string.Empty)
				{
					inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeHolder;
				}
			});

			return inputField;
		}

		else { DebugManager.Log($"WARNING : {_hierarchyName} is not in this hierarchy."); return null; }
	}

	public ScrollRect GetUI_ScrollRect(string _hierarchyName, Action<Vector2> _action = null)
	{
		if (childUI.ContainsKey(_hierarchyName))
		{
			var scrollRect = childUI[_hierarchyName].GetComponent<ScrollRect>();

			scrollRect.onValueChanged.AddListener((position) => _action?.Invoke(position));

			return scrollRect;
		}

		else { DebugManager.Log($"WARNING : {_hierarchyName} is not in this hierarchy."); return null; }
	}

	#endregion



	#region Utils

	public void ChangeTab(string _hierarchyName)
	{
		CloseTabAll();

		childUI[_hierarchyName].SetActive(true);
	}

	public void CloseTab(string _hierarchyName)
	{
		childUI[_hierarchyName].SetActive(false);
	}

	public T ChangeTab<T>() where T : Component
	{
		CloseTabAll();

		if (childUI.ContainsKey(typeof(T).Name))
		{
			childUI[typeof(T).Name].SetActive(true);
			return childUI[typeof(T).Name].GetComponent<T>();
		}
		else
		{
			DebugManager.Log($"WARNING: {typeof(T).Name} not found.");
			return null;
		}
	}

	public T FetchTab<T>() where T:Component
	{
		return childUI[typeof(T).Name].GetComponent<T>();
	}

	public void CloseTab<T>() where T : Component
	{
		if (childUI.ContainsKey(typeof(T).Name))
		{
			childUI[typeof(T).Name].SetActive(false);
		}

		else DebugManager.Log($"WARNING: {typeof(T).Name} not found.");
	}

	public void CloseTabAll()
	{
		childUI.Values
			.Where(uiObject => uiObject != null && uiObject.GetComponent<Tab_Base>() != null)
			.ToList()
			.ForEach(tabObject => tabObject.SetActive(false));
	}


	protected void OpenSound() => GameManager.Sound.PlaySound(Define.SOUND_OPEN);
	protected void CloseSound() => GameManager.Sound.PlaySound(Define.SOUND_CLOSE);

	protected void Display()
	{
		foreach (var item in childUI)
		{
			DebugManager.Log($"Key: {item.Key}, Value: {item.Value}");
		}
	}

	#endregion
}