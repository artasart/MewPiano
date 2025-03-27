using UnityEngine;


public class Scene_Title : Scene_Default
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();

		GameManager.UI.StartPanel<Panel_Title>(true);

		GameManager.UI.LastPopup<Popup_Default>();
	}
}
