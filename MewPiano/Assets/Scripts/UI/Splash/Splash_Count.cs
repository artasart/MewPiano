using DG.Tweening;
using MEC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Splash_Count : Splash_Base
{
	TMP_Text[] countTexts;
	TMP_Text txtmp_Count_1;
	TMP_Text txtmp_Count_2;
	TMP_Text txtmp_Count_3;
	TMP_Text txtmp_Message;

	public int count = 3;
	public float moveDuration = 0.5f;
	public float fadeDuration = 0.3f;

	private Dictionary<TMP_Text, Vector3> originalPositions = new(); // 원래 위치 저장

	protected override void Awake()
	{
		base.Awake();

		txtmp_Count_1 = GetUI_TMPText(nameof(txtmp_Count_1));
		txtmp_Count_2 = GetUI_TMPText(nameof(txtmp_Count_2));
		txtmp_Count_3 = GetUI_TMPText(nameof(txtmp_Count_3));

		countTexts = new TMP_Text[3]
		{
			txtmp_Count_1,
			txtmp_Count_2,
			txtmp_Count_3
		};

		txtmp_Message = GetUI_TMPText(nameof(txtmp_Message));

		foreach (var text in countTexts)
		{
			originalPositions[text] = text.transform.localPosition;
		}
		originalPositions[txtmp_Message] = txtmp_Message.transform.localPosition;
	}

	public void StartCount()
	{
		foreach (var text in countTexts)
		{
			text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
			text.gameObject.SetActive(false);
		}
		txtmp_Message.color = new Color(txtmp_Message.color.r, txtmp_Message.color.g, txtmp_Message.color.b, 0);
		txtmp_Message.gameObject.SetActive(false);

		Util.RunCoroutine(Co_StartCount(), nameof(Co_StartCount));
	}

	private IEnumerator<float> ShowText(TMP_Text text)
	{
		text.gameObject.SetActive(true);
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

		Vector3 basePos = originalPositions[text]; // 원래 위치 불러오기
		Vector3 startPos = basePos + new Vector3(0, 200f, 0);
		Vector3 endPos = basePos + new Vector3(0, -300f, 0);

		text.transform.localPosition = startPos;

		Sequence seq = DOTween.Sequence();
		seq.Append(text.DOFade(1, fadeDuration));
		seq.Join(text.transform.DOLocalMove(basePos, moveDuration));
		yield return Timing.WaitForSeconds(moveDuration + 0.5f);

		seq = DOTween.Sequence();
		seq.Append(text.DOFade(0, fadeDuration));
		seq.Join(text.transform.DOLocalMove(endPos, moveDuration));
		yield return Timing.WaitForSeconds(moveDuration);

		text.gameObject.SetActive(false);
	}

	private IEnumerator<float> Co_StartCount()
	{
		for (int i = 0; i < countTexts.Length; i++)
		{
			yield return Timing.WaitUntilDone(ShowText(countTexts[i]));
		}

		yield return Timing.WaitUntilDone(ShowText(txtmp_Message));

		GameManager.UI.PopSplash();

		GameManager.UI.lockBackey = false;
	}

	private void OnDisable()
	{
		foreach (var text in countTexts)
		{
			if (originalPositions.ContainsKey(text))
				text.transform.localPosition = originalPositions[text];
		}

		if (originalPositions.ContainsKey(txtmp_Message))
			txtmp_Message.transform.localPosition = originalPositions[txtmp_Message];
	}
}
