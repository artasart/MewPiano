using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PianoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Sprite normalSprite;  // 평소 상태 이미지
    public Sprite pressedSprite; // 클릭 시 변경할 이미지

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
            buttonImage.sprite = pressedSprite; // 클릭 시 이미지 변경
        }
    }

    // 버튼을 뗐을 때 호출됨
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite; // 원래 이미지로 복귀
        }
    }
}
