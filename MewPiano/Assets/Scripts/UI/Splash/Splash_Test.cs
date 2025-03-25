using UnityEngine;
using UnityEngine.UI;

public class Splash_Test : Splash_Base
{
	Button btn_Test;

	protected override void Awake()
	{
		base.Awake();

		btn_Test = GetUI_Button(nameof(btn_Test), OnClick_Test);
	}

	private void OnClick_Test()
	{
		GameManager.UI.PopSplash();
	}
}
