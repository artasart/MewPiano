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
    }
}
