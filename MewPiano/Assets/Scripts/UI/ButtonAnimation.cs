using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using MEC;
using static EasingFunction;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Ease easeFunction = Ease.EaseOutBack;
	public float targetScale = .95f;

	public bool useTextHover = false;
	public bool useAnimation = true;
	public Color defaultTextColor = Color.white;
	public Color highlightTextColor = Color.yellow;


	private Button button;
	public TMP_Text buttonText { get; set; }

	private void OnDestroy()
	{
		Util.KillCoroutine(nameof(Co_SizeTransform) + this.GetHashCode());
		Util.KillCoroutine(nameof(Co_TextColorLerp) + this.GetHashCode());
	}

	void Awake()
	{
		button = GetComponent<Button>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!useAnimation) return;

		Util.RunCoroutine(Co_SizeTransform(targetScale), nameof(Co_SizeTransform) + this.GetHashCode());
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!useAnimation) return;

		Util.RunCoroutine(Co_SizeTransform(1f), nameof(Co_SizeTransform) + this.GetHashCode());
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!useTextHover || buttonText == null) return;

		Util.RunCoroutine(Co_TextColorLerp(highlightTextColor), nameof(Co_TextColorLerp) + this.GetHashCode());
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!useTextHover || buttonText == null) return;

		Util.RunCoroutine(Co_TextColorLerp(defaultTextColor), nameof(Co_TextColorLerp) + this.GetHashCode());
	}

	private IEnumerator<float> Co_SizeTransform(float _size)
	{
		Vector3 current = button.GetComponent<RectTransform>().localScale;
		float lerpvalue = 0f;

		while (lerpvalue <= 1f)
		{
			if (button == null || button.GetComponent<RectTransform>() == null) yield break;

			Function function = GetEasingFunction(easeFunction);
			float x = function(current.x, _size, lerpvalue);
			float y = function(current.y, _size, lerpvalue);
			float z = function(current.z, _size, lerpvalue);

			lerpvalue += 3f * Time.deltaTime;
			button.GetComponent<RectTransform>().localScale = new Vector3(x, y, z);

			yield return Timing.WaitForOneFrame;
		}

		button.GetComponent<RectTransform>().localScale = Vector3.one * _size;
	}

	private IEnumerator<float> Co_TextColorLerp(Color targetColor)
	{
		if (buttonText == null) yield break;

		Color startColor = buttonText.color;
		float lerpValue = 0f;

		while (lerpValue <= 1f)
		{
			if (buttonText == null) yield break;

			buttonText.color = Color.Lerp(startColor, targetColor, lerpValue);
			lerpValue += 10f * Time.deltaTime;

			yield return Timing.WaitForOneFrame;
		}

		buttonText.color = targetColor;
	}
}
