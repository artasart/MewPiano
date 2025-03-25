using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(CanvasAreaController))]
public sealed class CanvasMasterController : MonoBehaviour
{
	#region Singleton

	public static CanvasMasterController Instance
	{
		get
		{
			if (instance != null) return instance;
			instance = FindObjectOfType<CanvasMasterController>();
			return instance;
		}
	}
	private static CanvasMasterController instance;

	#endregion




	#region Members

	private Canvas canvas;
	private CanvasScaler canvasScaler;
	private CanvasGroup canvasGroup;

	public bool manuallySetVisible = false;

	public Vector2 refrenceResolution = new Vector2(1920, 1080);

	public GraphicRaycaster graphicRaycaster { get; private set; }

	#endregion




	#region Initialize

	private void OnRectTransformDimensionsChange()
	{
		SetMatch();
	}

	private void Awake()
	{
		instance = this;

		canvas = GetComponent<Canvas>();
		canvasGroup = GetComponent<CanvasGroup>();
		canvasScaler = GetComponent<CanvasScaler>();
		graphicRaycaster = GetComponent<GraphicRaycaster>();

		SetUIVisible(false);

		Initialize();
	}

	private void Initialize()
	{
		canvas.sortingOrder = 0;

		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(refrenceResolution.x, refrenceResolution.y);
		canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

		SetMatch();

		canvasScaler.referencePixelsPerUnit = 100;

		if (!manuallySetVisible) SetUIVisible(true);
	}

	public void SetMatch()
	{
		if (!canvasScaler) return;

		float match = 1;

		var screenRatio = (float)Screen.width / (float)Screen.height;
		var scalerRatio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;

		if (screenRatio > scalerRatio)
		{
			match = 1.0f;
		}
		else
		{
			match = 0f;
		}

		if (canvasScaler) canvasScaler.matchWidthOrHeight = match;
	}

	public void SetUIVisible(bool enable)
	{
		canvasGroup.alpha = enable ? 1 : 0;
		canvasGroup.interactable = enable;
		canvasGroup.blocksRaycasts = enable;

		GetComponent<GraphicRaycaster>().enabled = enable;
	}

	#endregion
}
