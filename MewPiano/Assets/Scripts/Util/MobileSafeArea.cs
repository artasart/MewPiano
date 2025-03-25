using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MobileSafeArea : MonoBehaviour
{
	RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		ApplySafeArea();
	}

	private void ApplySafeArea()
	{
		Rect safeArea = Screen.safeArea;

		Vector2 anchorMin = safeArea.position;
		Vector2 anchorMax = safeArea.position + safeArea.size;

		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;

		_rectTransform.anchorMin = anchorMin;
		_rectTransform.anchorMax = anchorMax;

		//Debug.Log($"Safe Area Applied: {safeArea}");
	}

	private void OnRectTransformDimensionsChange()
	{
		if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
		ApplySafeArea();
	}
}
