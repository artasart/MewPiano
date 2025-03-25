using UnityEngine;

public sealed class CanvasAreaController : MonoBehaviour
{
	private Canvas canvas;
	private RectTransform rectTransform;
	private Rect currentSafeArea = new Rect();

	ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;

	void Start()
	{
		canvas = GetComponent<Canvas>();
		rectTransform = GetComponent<RectTransform>();

		currentOrientation = Screen.orientation;
		currentSafeArea = Screen.safeArea;

		ApplySafeArea();
	}

	private void ApplySafeArea()
	{
		if (rectTransform == null) return;

		Rect safeArea = Screen.safeArea;

		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position = safeArea.size;

		anchorMin.x /= canvas.pixelRect.width;
		anchorMin.y /= canvas.pixelRect.height;

		anchorMax.x /= canvas.pixelRect.width;
		anchorMax.y /= canvas.pixelRect.height;

		rectTransform.anchorMin = anchorMin;
		rectTransform.anchorMax = anchorMax;
	}

	//void Update()
	//{
	//	if ((currentOrientation != Screen.orientation) || (currentSafeArea != Screen.safeArea))
	//	{
	//		ApplySafeArea();
	//		currentOrientation = Screen.orientation;

	//		Debug.Log("?");
	//	}
	//}
}
