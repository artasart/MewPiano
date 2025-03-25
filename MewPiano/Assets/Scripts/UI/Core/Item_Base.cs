using System;
using UnityEngine.UI;

public class Item_Base : UI_Base
{

	protected override void Awake()
	{
		base.Awake();
	}

	public Button GetItem_Button(Action _action = null, Action _sound = null, bool useSound = true, bool useAnimation = false)
	{
		var button = this.GetComponent<Button>();

		if (_sound == null)
		{
			if (useSound)
			{
				button.onClick.AddListener(OpenSound);
			}
		}

		else button.onClick.AddListener(() => _sound?.Invoke());

		button.onClick.AddListener(() => _action?.Invoke());

		if (useAnimation) button.UseAnimation();

		return button;
	}
}