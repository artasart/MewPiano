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
    }
}
