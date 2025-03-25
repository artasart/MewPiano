using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Main : Panel_Base
{
    TMP_Text main_text;
    Button Button_1;
    Button Button_2;
    Button Button_3;



    protected override void Awake()
    {
        base.Awake();
        Button_1 = GetUI_Button(nameof(Button_1), OnClick_Test);
        Button_2 = GetUI_Button(nameof(Button_2), OnClick_Test2);
        Button_3 = GetUI_Button(nameof(Button_3), OnClick_Test3);
    }

    void Start()
    {
        
    }

    private void OnClick_Test()
    {
        Debug.Log("��ư Ŭ�� �� �� �̵�");

        GameManager.Scene.LoadScene(Enums.SceneName.Logo);
    }

    private void OnClick_Test2()
    {
        Debug.Log("��ư Ŭ�� �� �˾� Ȱ��ȭ");
        GameManager.UI.StackPopup<Popup_Default>();
    }

    private void OnClick_Test3()
    {
        Debug.Log("��ư Ŭ�� �� �� �̵�");
        GameManager.Scene.LoadScene(Enums.SceneName.Title);
    }
}
