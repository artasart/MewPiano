using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PianoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Sprite normalSprite;  // 평소 상태 이미지
    public Sprite pressedSprite; // 클릭 시 변경할 이미지

    private bool isPressed = false; // 버튼이 눌린 상태
    private int buttonIndex; // 해당 건반의 인덱스
    private bool isBlackKey;  // 검은 건반 여부

    private string keyName;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();

        // 초기 이미지 설정
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }
    }

    // 버튼을 눌렀을 때 호출됨
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonImage != null && pressedSprite != null)
        {
            buttonImage.sprite = pressedSprite;
        }

        isPressed = true;


        if (isBlackKey)
        {
            Debug.Log($"검은 건반 {buttonIndex} 번째를 누름.");
        }
        else
        {
            Debug.Log($"흰 건반 {buttonIndex} 번째를 누름.");
        }
    }


    // 버튼을 뗐을 때 호출됨
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite;
        }

        if (!isBlackKey)
        {
            Debug.Log($"흰 건반 {buttonIndex} 번째를 뗌.");
        }
        else
        {
            Debug.Log($"검은 건반 {buttonIndex} 번째를 뗌.");
        }

        isPressed = false;
    }

    // 버튼을 설정할 때 인덱스와 건반 종류를 할당하는 함수
    public void SetButtonIndex(int index, bool isBlack)
    {
        buttonIndex = index;  // 버튼의 인덱스 설정
        isBlackKey = isBlack;  // 검은 건반 여부 설정
        keyName = GetAudioFileName(index, isBlack);

        GetComponent<Button>().onClick.AddListener(PlayNote);

    }

    private string GetAudioFileName(int index, bool isBlack)
    {
        // 기본 음계 (흰 건반)
        string[] whiteNotes = { "a", "b", "c", "d", "e", "f", "g" };
        // 검은 건반 (#) (C#, D#, F#, G#, A#)
        string[] blackNotes = { "a_sharp", "", "c_sharp", "d_sharp", "", "f_sharp", "g_sharp" };

        int noteIndex = index % 7;  // 현재 계이름 위치
        int octave = index / 12;    // 옥타브 계산

        string noteName = isBlack ? blackNotes[noteIndex] : whiteNotes[noteIndex];

        // 검은 건반 중 빈칸이면 (E#, B# 존재 X)
        if (string.IsNullOrEmpty(noteName)) return "";

        return $"key{index:D2}_{noteName}{octave}";

       
    }

    public void PlayNote()
    {
        if (!string.IsNullOrEmpty(keyName))
        {
            Debug.Log($"재생 요청: {keyName}");
            SoundManager.instance.PlayPianoSound(keyName);
        }
    }
}
