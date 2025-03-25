using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Demo : Scene_Default
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        GameManager.UI.StartPanel<Panel_Demo_Title>(true);

      //  GameManager.UI.LastPopup<Popup_Default>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
