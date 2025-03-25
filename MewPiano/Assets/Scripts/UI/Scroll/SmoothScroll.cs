using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

public class SmoothScroll : MonoBehaviour
{
	public EnhancedScroller scroller;
	public float smoothTime = 0.05f;
	public float scrollSpeed = 300f;

	private float targetPosition;
	private float velocity = 0f;
	private bool isScrollingWithWheel = false;

	private void Start()
	{
		// 🎯 현재 플랫폼이 모바일이면 스크립트 제거
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Destroy(this); // 이 스크립트 제거
			return;
		}

		else
		{
			GetComponent<ScrollRect>().enabled = false;
		}

		// PC인 경우만 정상 동작
		scroller = GetComponent<EnhancedScroller>();
	}

	void Update()
	{
		float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

		if (Mathf.Abs(scrollDelta) > 0.001f)
		{
			isScrollingWithWheel = true;
			targetPosition = scroller.ScrollPosition + scrollDelta * -scrollSpeed;
		}

		if (isScrollingWithWheel)
		{
			float newScrollPosition = Mathf.SmoothDamp(scroller.ScrollPosition, targetPosition, ref velocity, smoothTime);
			scroller.SetScrollPositionImmediately(newScrollPosition);

			if (Mathf.Abs(scroller.ScrollPosition - targetPosition) < 0.1f)
			{
				isScrollingWithWheel = false;
			}
		}
	}
}