using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Item_Message : Item_Base
{
	TMP_Text txtmp_Message;

	RectTransform rectTransform;
	CanvasGroup canvasgroup;

	protected override void Awake()
	{
		base.Awake();

		txtmp_Message = GetUI_TMPText(nameof(txtmp_Message));

		rectTransform = this.GetComponent<RectTransform>();
		canvasgroup = this.GetComponent<CanvasGroup>();

		Init();
	}

	public void SetMessage(string message)
	{
		txtmp_Message.text = message;
	}

	public void Init()
	{
		txtmp_Message.text = string.Empty;
		canvasgroup.alpha = 0f;
		rectTransform.anchoredPosition = Vector2.up * 250f;
	}

	public void Show()
	{
		CancelInvoke(nameof(Disappear));

		var positionY = this.GetComponent<RectTransform>().localPosition.y + 25;

		rectTransform.DOAnchorPosY(positionY, .75f).OnComplete(() =>
		{
			Invoke(nameof(Disappear), .25f);
		});

		canvasgroup.DOFade(1f, .25f);
	}

	private void Disappear()
	{
		canvasgroup.DOFade(0f, .25f).OnComplete(() =>
		{
			Destroy(this.gameObject);
		});
	}
}