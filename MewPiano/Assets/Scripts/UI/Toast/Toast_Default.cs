using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast_Default : UI_Base
{
	Button btn_ToastPopup;

	TMP_Text txtmp_ToastMessage;

	protected override void Awake()
	{
		base.Awake();

		btn_ToastPopup = this.GetComponent<Button>();
		btn_ToastPopup.onClick.AddListener(OnClick_ToastPopup);
		btn_ToastPopup.UseAnimation();

		txtmp_ToastMessage = GetUI_TMPText(nameof(txtmp_ToastMessage), string.Empty);
	}

	private void Start()
	{
		Hide(true);
	}

	private void OnClick_ToastPopup()
	{
		Hide();
	}

	public void Show(string message)
	{
		GameManager.Sound.PlaySound(Define.SOUND_TOAST);

		txtmp_ToastMessage.text = message;

		this.GetComponent<RectTransform>().DOLocalMove(new Vector3(0f, 300, 0f), .5f);
	}

	public void Hide(bool isInstant = false)
	{
		GameManager.Sound.PlaySound(Define.SOUND_CLOSE);

		if (isInstant)
		{
			this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 490, 0f);

			return;
		}

		this.GetComponent<RectTransform>().DOLocalMove(new Vector3(0f, 490, 0f), .5f).OnComplete(() => txtmp_ToastMessage.text = string.Empty);
	}
}
