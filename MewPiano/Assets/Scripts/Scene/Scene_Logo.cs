using UnityEngine;

public class Scene_Logo : Scene_Default
{
	public string name = "cwj";

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();

		GameManager.UI.StartPanel<Panel_Logo>(true);

		GameManager.UI.LastPopup<Popup_Default>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Z))
		{
			GameManager.UI.StackPanel<Panel_Test>(true).UpdateInfo(name);
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			GameManager.UI.PopPanel();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameManager.UI.StackPopup<Popup_Default>(true).SetPopupInfo(ModalType.Confrim, "teadfasdfasdfst", "tset", 
			() =>
			{
				Debug.Log("확인");
			}, 
			() =>{
				Debug.Log("취소");
			});
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameManager.UI.PopPopup();
		}
	}
}
