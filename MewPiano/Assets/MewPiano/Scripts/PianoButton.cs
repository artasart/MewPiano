using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PianoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Sprite normalSprite;  // ��� ���� �̹���
    public Sprite pressedSprite; // Ŭ�� �� ������ �̹���

    private bool isPressed = false; // ��ư�� ���� ����
    private int buttonIndex; // �ش� �ǹ��� �ε���
    private bool isBlackKey;  // ���� �ǹ� ����

    private string keyName;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();

        // �ʱ� �̹��� ����
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    // ��ư�� ������ �� ȣ���
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }

        isPressed = true;


        if (isBlackKey)
        {
            Debug.Log($"���� �ǹ� {buttonIndex} ��°�� ����.");
        }
        else
        {
            Debug.Log($"�� �ǹ� {buttonIndex} ��°�� ����.");
        }
    }


    // ��ư�� ���� �� ȣ���
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }

        if (!isBlackKey)
        {
            Debug.Log($"�� �ǹ� {buttonIndex} ��°�� ��.");
        }
        else
        {
            Debug.Log($"���� �ǹ� {buttonIndex} ��°�� ��.");
        }

        isPressed = false;
    }

    // ��ư�� ������ �� �ε����� �ǹ� ������ �Ҵ��ϴ� �Լ�
    public void SetButtonIndex(int index, bool isBlack)
    {
        buttonIndex = index;  // ��ư�� �ε��� ����
        isBlackKey = isBlack;  // ���� �ǹ� ���� ����
        keyName = GetAudioFileName(index, isBlack);

        GetComponent<Button>().onClick.AddListener(PlayNote);

    }

    private string GetAudioFileName(int index, bool isBlack)
    {
        // �⺻ ���� (�� �ǹ�)
        string[] whiteNotes = { "a", "b", "c", "d", "e", "f", "g" };
        // ���� �ǹ� (#) (C#, D#, F#, G#, A#)
        string[] blackNotes = { "a_sharp", "", "c_sharp", "d_sharp", "", "f_sharp", "g_sharp" };

        int noteIndex = index % 7;  // ���� ���̸� ��ġ
        int octave = index / 12;    // ��Ÿ�� ���

        string noteName = isBlack ? blackNotes[noteIndex] : whiteNotes[noteIndex];

        // ���� �ǹ� �� ��ĭ�̸� (E#, B# ���� X)
        if (string.IsNullOrEmpty(noteName)) return "";

        return $"key{index:D2}_{noteName}{octave}";

       
    }

    public void PlayNote()
    {
        if (!string.IsNullOrEmpty(keyName))
        {
            Debug.Log($"��� ��û: {keyName}");
            SoundManager.instance.PlayPianoSound(keyName);
        }
    }
}
