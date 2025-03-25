using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Demo_Title : Panel_Base
{
    Button Button_Facebook;
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick_Facebook()
    {
        Debug.Log("버튼 클릭");
        GameManager.UI.StackPanel<Panel_Login>(true);
    }
}
