using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Last : Panel_Base
{
    public GameObject pianoObject;  // 건반 프리팹
    public Transform pianoParent;   // 건반이 들어갈 부모 패널

    public GameObject pianoObject_black;
    public Transform piano_Black_Parent;

    public GameObject noteObject;   // 계이름 (PNG) 프리팹
    public Transform noteParent;    // 계이름이 들어갈 부모 패널
    public Transform noteParent_Black;

    public GameObject noteObject_S; 
    public Transform noteParent_S;  

    protected override void Awake()
    {
        base.Awake();
    }

    public int pianoCount = 44;  // 건반 개수
    public int startIndex = 3;
    public Sprite[] noteSprites; // 계이름 이미지 배열 (C~B)

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DestroyChild();

            for (int i = 0; i < pianoCount; i++)
            {
                MakePiano(i);
                MakePiano_Black(i);
                MakeNote(i);
                MakeNote_S(i);
            }
        }
    }

    public void MakePiano(int index)
    {
        var piano = Instantiate(pianoObject, pianoParent);
        piano.transform.localPosition = Vector3.zero;
        piano.transform.localScale = Vector3.one;
        piano.GetComponent<Button>().onClick.AddListener(() => Onclick_Piano(index));
    }

    public void MakePiano_Black(int index)
    {
        var piano_black = Instantiate(pianoObject_black, piano_Black_Parent);
        piano_black.transform.localPosition = Vector3.zero;
        piano_black.transform.localScale = Vector3.one;
        piano_black.GetComponent<Button>().onClick.AddListener(() => Onclick_Piano(index));
    }

    public void MakeNote(int index)
    {
        var note = Instantiate(noteObject, noteParent);
        note.transform.localPosition = Vector3.zero;
        note.transform.localScale = Vector3.one;

        var note_black = Instantiate(noteObject, noteParent_Black);
        note_black.transform.localPosition = Vector3.zero;
        note_black.transform.localScale = Vector3.one;

        // 시작 인덱스 설정
        int noteIndex = (startIndex + index) % noteSprites.Length;
        note.GetComponent<Image>().sprite = noteSprites[noteIndex];
        note_black.GetComponent<Image>().sprite = noteSprites[noteIndex];
    }


    public void MakeNote_S(int index)
    {
        var note_S = Instantiate(noteObject_S, noteParent_S); 
        note_S.transform.localPosition = Vector3.zero;
        note_S.transform.localScale = Vector3.one;

    }

    private void Onclick_Piano(int index)
    {
        Debug.Log($"{index} 번째 건반 눌림");
    }

    private void DestroyChild()
    {
        foreach (Transform item in pianoParent)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in noteParent)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in noteParent_Black)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in piano_Black_Parent)
        {
            Destroy(item.gameObject);
        }

     
        foreach (Transform item in noteParent_S)
        {
            Destroy(item.gameObject);
        }
    }
}
