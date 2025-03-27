using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Test : Panel_Base
{
	TMP_Text txtmp_Name;

	Button btn_Test;

	protected override void Awake()
	{
		base.Awake();

		txtmp_Name = GetUI_TMPText(nameof(txtmp_Name));
		btn_Test = GetUI_Button(nameof(btn_Test), OnClick_Test);
	}

	private void Start()
	{
		
	}

	public void UpdateInfo(string name)
	{
		Debug.Log(name);

		txtmp_Name.text = name;
	}

	private void OnClick_Test()
	{
		Debug.Log("버튼 클릭");

		GameManager.Scene.LoadScene(Enums.SceneName.Title);
	}

}
