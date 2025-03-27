using UnityEngine;
using UnityEngine.UI;

public class Piano_White_Manager : MonoBehaviour
{
    public GameObject keyPrefab; // Piano key prefab
    public Transform panelTransform; // Panel�� RectTransform
    public int totalKeys = 44; // �⺻��: 44���� �ǹ�
    
    void Start()
    {
        CreatePianoKeys();
    }

    void CreatePianoKeys()
    {
        for (int i = 0; i < totalKeys; i++)
        {
            // Ű���� �ǹ� �������� ����
            GameObject key = Instantiate(keyPrefab, panelTransform);

            // �ǹ��� RectTransform�� ��������
            RectTransform keyRect = key.GetComponent<RectTransform>();

        }
    }
}
