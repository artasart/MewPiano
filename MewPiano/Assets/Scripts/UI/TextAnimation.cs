using UnityEngine;
using System.Collections.Generic;
using MEC;
using TMPro;
using static Enums;

public class TextAnimation : MonoBehaviour
{
	TMP_Text txtmp;
	public CanvasGroup canvasGroup;

	public float start = 1f; // 시작 알파값
	public float end = .4f; // 끝 알파값

	private void OnDestroy()
	{
		Util.KillCoroutine(nameof(Co_PingPong) + this.GetHashCode());
	}

	private void Awake()
	{
		txtmp = GetComponent<TMP_Text>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void SetAlphaRange(int start, int end)
	{
		this.start = start;
		this.end = end;
	}

	public void StartPingPong(float pingpongSpeed = 1f)
	{
		Util.RunCoroutine(Co_PingPong(pingpongSpeed), nameof(Co_PingPong) + this.GetHashCode(), CoroutineTag.UI);
	}

	private IEnumerator<float> Co_PingPong(float pingpongSpeed = 1f)
	{
		canvasGroup = GetComponent<CanvasGroup>();

		while (true)
		{
			float alpha = Mathf.PingPong(Time.time * pingpongSpeed, 1f);

			alpha = Mathf.Lerp(start, end, alpha);

			if (canvasGroup != null) canvasGroup.alpha = alpha;

			yield return Timing.WaitForOneFrame;
		}
	}

	public void StopPingPong()
	{
		Util.KillCoroutine(nameof(Co_PingPong) + this.GetHashCode());

		Util.FadeCanvasGroup(canvasGroup, 1f);
	}
}
