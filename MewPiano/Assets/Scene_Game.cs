using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Game : Scene_Default
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        GameManager.UI.StartPanel<Panel_Game>(true);

        GameManager.UI.LastPopup<Popup_Default>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.UI.StackPanel<Panel_Main>(true);
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
            () => {
                Debug.Log("취소");
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.UI.PopPopup();
        }
    }

}
