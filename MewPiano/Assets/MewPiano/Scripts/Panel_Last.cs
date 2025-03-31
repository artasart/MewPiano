using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Last : Panel_Base
{
    public GameObject pianoObject;  // �ǹ� ������
    public Transform pianoParent;   // �ǹ��� �� �θ� �г�

    public GameObject pianoObject_black;
    public Transform piano_Black_Parent;

    protected override void Awake()
    {
        base.Awake();
    }

    public int pianoCount = 44;  // �ǹ� ����
    public int startIndex = 3;

    // ���̸� ���� �迭
    private string[] noteNames = new string[] { "C", "D", "E", "F", "G", "A", "B" };

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DestroyChild();

            for (int i = 0; i < pianoCount; i++)
            {
                MakePiano(i);
                MakePiano_Black(i);
            }
        }
    }

    public void MakePiano(int index)
    {
        var piano = Instantiate(pianoObject, pianoParent);
        piano.transform.localPosition = Vector3.zero;
        piano.transform.localScale = Vector3.one;

        int noteIndex = (startIndex + index) % noteNames.Length;
        string noteName = noteNames[noteIndex];

        TextMeshProUGUI buttonText = piano.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = noteName;
        }

        // �� �ǹݿ� ���� ����
        var pianoButton = piano.GetComponent<PianoButton>();
        if (pianoButton != null)
        {
            pianoButton.SetButtonIndex(index, false);  // �� �ǹ��� false
        }

    }

    public void MakePiano_Black(int index)
    {
        int notePosition = (startIndex + index) % 7;
        int noteIndex = (startIndex + index) % noteNames.Length;
        string noteName = noteNames[noteIndex];

        if (notePosition == 2 || notePosition == 6)
        {
            // �� ���� ������Ʈ�� �߰��ؼ� ���� ����
            var emptySpace = new GameObject("EmptySpace");
            emptySpace.transform.SetParent(piano_Black_Parent);
            emptySpace.AddComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            return;
        }

        var piano_black = Instantiate(pianoObject_black, piano_Black_Parent);
        piano_black.transform.localPosition = Vector3.zero;
        piano_black.transform.localScale = Vector3.one;


        TextMeshProUGUI[] buttonTexts = piano_black.GetComponentsInChildren<TextMeshProUGUI>();
        if (buttonTexts.Length > 1)
        {
            // #���� ���̸��� ����
            TextMeshProUGUI buttonText = buttonTexts[0];
            buttonText.text = noteName;
        }

        // ���� �ǹݿ� ���� ����
        var pianoButtonBlack = piano_black.GetComponent<PianoButton>();
        if (pianoButtonBlack != null)
        {
            pianoButtonBlack.SetButtonIndex(index, true);  // ���� �ǹ��� true
        }
    }


    private void DestroyChild()
    {
        foreach (Transform item in pianoParent)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in piano_Black_Parent)
        {
            Destroy(item.gameObject);
        }
    }
}
