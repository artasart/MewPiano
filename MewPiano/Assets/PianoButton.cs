using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PianoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image buttonImage;
    public Sprite normalSprite;  // ��� ���� �̹���
    public Sprite pressedSprite; // Ŭ�� �� ������ �̹���

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
            buttonImage.sprite = pressedSprite; // Ŭ�� �� �̹��� ����
        }
    }

    // ��ư�� ���� �� ȣ���
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonImage != null && normalSprite != null)
        {
            buttonImage.sprite = normalSprite; // ���� �̹����� ����
        }
    }
}
