using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Last : Panel_Base
{
    public GameObject pianoObject;  // �� �ǹ� ������
    public Transform pianoParent;   // �� �ǹ��� �� �θ� �г�

    public GameObject pianoObject_black;  // ���� �ǹ� ������
    public Transform piano_Black_Parent;  // ���� �ǹ��� �� �θ� �г�

 /*   public GameObject guideLinePrefab; // ���̵���� ������
    public Transform guideLineParent; // ���̵������ �� �θ� �г�
 */
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

        var pianoButton = piano.GetComponent<PianoButton>();
        if (pianoButton != null)
        {
            pianoButton.SetButtonIndex(index, false);  // �� �ǹ��� false
        }

        /* �� �ǹ� ������ ���̵���� �߰�
        var guideLine = Instantiate(guideLinePrefab, guideLineParent);
        guideLine.transform.localPosition = Vector3.zero;
        guideLine.transform.localScale = Vector3.one;*/
    }

    public void MakePiano_Black(int index)
    {
        int notePosition = (startIndex + index) % 7;
        int noteIndex = (startIndex + index) % noteNames.Length;
        string noteName = noteNames[noteIndex];

        // ���� �ǹ��� ���� �� (��#�� ��#)�̰ų� ������ �ǹ��� ���
        bool isTransparentKey = (notePosition == 2 || notePosition == 6 || index == pianoCount - 1);

        var piano_black = Instantiate(pianoObject_black, piano_Black_Parent);
        piano_black.transform.localPosition = Vector3.zero;
        piano_black.transform.localScale = Vector3.one;

        // ��ư �ؽ�Ʈ ���� (�ش� �ǹ� ��Ȱ��ȭ)
        TextMeshProUGUI[] buttonTexts = piano_black.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in buttonTexts)
        {
            if (isTransparentKey)
            {
                text.gameObject.SetActive(false); // �ؽ�Ʈ ��Ȱ��ȭ
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
                transparentColor.a = 0f;  // ���İ� 0���� ����
                keyImage.color = transparentColor;
                keyImage.raycastTarget = false; // Raycast Target ��Ȱ��ȭ
            }
        }

        var pianoButtonBlack = piano_black.GetComponent<PianoButton>();
        if (pianoButtonBlack != null)
        {
            pianoButtonBlack.SetButtonIndex(index, true);  // ���� �ǹ��� true
        }
    }

    private void DestroyChild()
    {
        Debug.Log("��� �ǹ� ���� �����");

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
        Debug.Log("���� �Ϸ� �� �� �ǹ� ���� ����");
    }
}