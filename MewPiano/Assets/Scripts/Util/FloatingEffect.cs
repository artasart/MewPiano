using UnityEngine;
using DG.Tweening;

public class FloatingEffect : MonoBehaviour
{
	public float floatStrength = 10f;
	public float duration = 1f;

	private float startY;

	RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = this.GetComponent<RectTransform>();
	}

	void Start()
	{
		if (rectTransform != null)
		{
			startY = rectTransform.anchoredPosition.y;

			StartFloating();
		}
	}

	public void StartFloating()
	{
		if (rectTransform == null) return;

		rectTransform.DOAnchorPosY(startY + floatStrength, duration)
			.SetEase(Ease.InOutSine)
			.SetLoops(-1, LoopType.Yoyo);
	}
}
