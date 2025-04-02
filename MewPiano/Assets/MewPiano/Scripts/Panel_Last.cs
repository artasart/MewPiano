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

 /*   public GameObject guideLinePrefab; // 가이드라인 프리팹
    public Transform guideLineParent; // 가이드라인이 들어갈 부모 패널
 */
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

        /* 흰 건반 위에만 가이드라인 추가
        var guideLine = Instantiate(guideLinePrefab, guideLineParent);
        guideLine.transform.localPosition = Vector3.zero;
        guideLine.transform.localScale = Vector3.one;*/
    }

    public void MakePiano_Black(int index)
    {
        int notePosition = (startIndex + index) % 7;
        int noteIndex = (startIndex + index) % noteNames.Length;
        string noteName = noteNames[noteIndex];

        // 검은 건반이 없는 음 (미#과 시#)이거나 마지막 건반일 경우
        bool isTransparentKey = (notePosition == 2 || notePosition == 6 || index == pianoCount - 1);

        var piano_black = Instantiate(pianoObject_black, piano_Black_Parent);
        piano_black.transform.localPosition = Vector3.zero;
        piano_black.transform.localScale = Vector3.one;

        // 버튼 텍스트 설정 (해당 건반 비활성화)
        TextMeshProUGUI[] buttonTexts = piano_black.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in buttonTexts)
        {
            if (isTransparentKey)
            {
                text.gameObject.SetActive(false); // 텍스트 비활성화
            }
            else
            {
                text.text = noteName;
            }
        }

        if (isTransparentKey)
        {
            Image keyImage = piano_black.GetComponent<Image>();
            if (keyImage != null)
            {
                Color transparentColor = keyImage.color;
                transparentColor.a = 0f;  // 알파값 0으로 설정
                keyImage.color = transparentColor;
                keyImage.raycastTarget = false; // Raycast Target 비활성화
            }
        }

        var pianoButtonBlack = piano_black.GetComponent<PianoButton>();
        if (pianoButtonBlack != null)
        {
            pianoButtonBlack.SetButtonIndex(index, true);  // 검은 건반은 true
        }
    }

    private void DestroyChild()
    {
        Debug.Log("모든 건반 삭제 실행됨");

        foreach (Transform item in pianoParent)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in piano_Black_Parent)
        {
            Destroy(item.gameObject);
        }

       /* foreach (Transform item in guideLineParent)
        {
            Destroy(item.gameObject);
        }
       */
        Debug.Log("삭제 완료 후 새 건반 생성 시작");
    }
}