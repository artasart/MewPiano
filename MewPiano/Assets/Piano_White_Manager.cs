using UnityEngine;
using UnityEngine.UI;

public class Piano_White_Manager : MonoBehaviour
{
    public GameObject keyPrefab; // Piano key prefab
    public Transform panelTransform; // Panel의 RectTransform
    public int totalKeys = 44; // 기본값: 44개의 건반
    
    void Start()
    {
        CreatePianoKeys();
    }

    void CreatePianoKeys()
    {
        for (int i = 0; i < totalKeys; i++)
        {
            // 키보드 건반 프리팹을 생성
            GameObject key = Instantiate(keyPrefab, panelTransform);

            // 건반의 RectTransform을 가져오기
            RectTransform keyRect = key.GetComponent<RectTransform>();

        }
    }
}
