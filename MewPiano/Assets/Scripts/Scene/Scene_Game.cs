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

        GameManager.UI.StartPanel<Panel_Last>(true);
    }
}
