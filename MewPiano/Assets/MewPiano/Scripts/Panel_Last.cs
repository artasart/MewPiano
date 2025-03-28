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

    public GameObject noteObject;   // ���̸� (PNG) ������
    public Transform noteParent;    // ���̸��� �� �θ� �г�
    public Transform noteParent_Black;

    public GameObject noteObject_S; 
    public Transform noteParent_S;  

    protected override void Awake()
    {
        base.Awake();
    }

    public int pianoCount = 44;  // �ǹ� ����
    public int startIndex = 3;
    public Sprite[] noteSprites; // ���̸� �̹��� �迭 (C~B)

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

        // ���� �ε��� ����
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
        Debug.Log($"{index} ��° �ǹ� ����");
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
