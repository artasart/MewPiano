using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum ModalType
{
	Confrim,
	ConfirmCancel,
}

public class Popup_Basic : Popup_Base
{
	Button btn_Confirm;
	Button btn_Close;

	TMP_Text txtmp_Notice;
	TMP_Text txtmp_Description;
	TMP_Text txtmp_Confirm;
	TMP_Text txtmp_Close;

	public ModalType modalType = ModalType.ConfirmCancel;

	private void OnEnable()
	{
		switch (modalType)
		{
			case ModalType.Confrim:
				btn_Confirm.gameObject.SetActive(true);
				btn_Back.gameObject.SetActive(false);
				break;
			case ModalType.ConfirmCancel:
				btn_Confirm.gameObject.SetActive(true);
				btn_Confirm.gameObject.SetActive(true);
				break;
		}
	}

	private void OnDisable()
	{
		Clear();
	}

	protected override void Awake()
	{
		base.Awake();

		btn_Confirm = GetUI_Button(nameof(btn_Confirm), OnClick_Confirm);
		btn_Back.onClick.RemoveAllListeners();
		btn_Dim.onClick.RemoveAllListeners();

		btn_Back = GetUI_Button(nameof(btn_Back), OnClick_Close, OpenSound);
		btn_Dim = GetUI_Button(nameof(btn_Dim), OnClick_Close, OpenSound);
		btn_Close = GetUI_Button(nameof(btn_Close), OnClick_Close, OpenSound);

		txtmp_Description = GetUI_TMPText(nameof(txtmp_Description), string.Empty);
		txtmp_Confirm = GetUI_TMPText(nameof(txtmp_Confirm));
		txtmp_Close = GetUI_TMPText(nameof(txtmp_Close));
		txtmp_Notice = GetUI_TMPText(nameof(txtmp_Notice));
	}

	public void SetPopupInfo(ModalType modalType, string description, string title = "", Action confirm = null, Action cancel = null)
	{
		this.modalType = modalType;
		txtmp_Description.text = description;

		callback_confirm = confirm;
		callback_cancel = cancel;

		if (!string.IsNullOrEmpty(title))
		{
			txtmp_Notice.text = title;
		}

		if (modalType == ModalType.Confrim)
		{
			btn_Back.gameObject.SetActive(false);
		}
	}

	public void Clear()
	{
		txtmp_Notice.text = "알림";
		txtmp_Description.text = "게임을 종료하시겠습니까?";
		callback_confirm = () => Application.Quit();
		callback_cancel = null;

		modalType = ModalType.ConfirmCancel;
		btn_Back.gameObject.SetActive(true);
		btn_Confirm.gameObject.SetActive(true);
	}
}