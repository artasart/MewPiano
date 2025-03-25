using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Default : Popup_Base
{
	Button btn_Confirm;
	Button btn_Close;

	TMP_Text txtmp_Title;
	TMP_Text txtmp_Description;

	public ModalType modalType = ModalType.ConfirmCancel;

	protected override void Awake()
	{
		isDefault = false;

		base.Awake();

		btn_Confirm = GetUI_Button(nameof(btn_Confirm), OnClick_Confirm, CloseSound);
		btn_Close = GetUI_Button(nameof(btn_Close), OnClick_Close, CloseSound);
		btn_Confirm.UseAnimation();
		btn_Close.UseAnimation();

		txtmp_Title = GetUI_TMPText(nameof(txtmp_Title), "시스템 알림");
		txtmp_Description = GetUI_TMPText(nameof(txtmp_Description), "게임을 종료하시겠습니까?");

		UseDimClose();

		isInstant = false;
	}

	protected override void OnClick_Confirm()
	{
		base.OnClick_Confirm();
	}

	protected override void OnClick_Close()
	{
		base.OnClick_Close();
	}

	public void SetPopupInfo(ModalType modalType, string description, string title = "", Action confirm = null, Action cancel = null)
	{
		this.modalType = modalType;
		txtmp_Description.text = description;

		callback_confirm = confirm;
		callback_cancel = cancel;

		if (!string.IsNullOrEmpty(title))
		{
			txtmp_Title.text = title;
		}

		if (modalType == ModalType.Confrim)
		{
			btn_Close.gameObject.SetActive(false);
			btn_Back.gameObject.SetActive(false);
		}
	}
}