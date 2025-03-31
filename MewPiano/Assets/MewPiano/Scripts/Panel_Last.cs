using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Last : Panel_Base
{
    public GameObject pianoObject;  // 흰 건반 프리팹
    public Transform pianoParent;   // 흰 건반이 들어갈 부모 패널

    public GameObject pianoObject_black;  // 검은 건반 프리팹
    public Transform piano_Black_Parent;  // 검은 건반이 들어갈 부모 패널

    protected override void Awake()
    {
        base.Awake();
    }

    public int pianoCount = 44;  // 건반 개수
    public int startIndex = 3;

    // 계이름 음계 배열
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

        var pianoButton = piano.GetComponent<PianoButton>();
        if (pianoButton != null)
        {
            pianoButton.SetButtonIndex(index, false);  // 흰 건반은 false
        }
    }

    public void MakePiano_Black(int index)
    {
        int notePosition = (startIndex + index) % 7;
        int noteIndex = (startIndex + index) % noteNames.Length;
        string noteName = noteNames[noteIndex];

        if (notePosition == 2 || notePosition == 6)
        {
            // 빈 투명 오브젝트를 추가해서 간격 유지
            var emptySpace = new GameObject("EmptySpace");
            emptySpace.transform.SetParent(piano_Black_Parent);
            RectTransform emptyRect = emptySpace.AddComponent<RectTransform>();

            // 검은 건반 크기와 동일하게 설정
            if (pianoObject_black != null)
            {
                RectTransform blackKeyRect = pianoObject_black.GetComponent<RectTransform>();
                if (blackKeyRect != null)
                {
                    emptyRect.sizeDelta = blackKeyRect.sizeDelta;
                    emptyRect.localScale = blackKeyRect.localScale;
                }
            }
            return;
        }

        var piano_black = Instantiate(pianoObject_black, piano_Black_Parent);
        piano_black.transform.localPosition = Vector3.zero;
        piano_black.transform.localScale = Vector3.one;

        TextMeshProUGUI[] buttonTexts = piano_black.GetComponentsInChildren<TextMeshProUGUI>();
        if (buttonTexts.Length > 1)
        {
            TextMeshProUGUI buttonText = buttonTexts[0];
            buttonText.text = noteName;
        }

        var pianoButtonBlack = piano_black.GetComponent<PianoButton>();
        if (pianoButtonBlack != null)
        {
            pianoButtonBlack.SetButtonIndex(index, true);  // 검은 건반은 true
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
