using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_piano : MonoBehaviour
{
    public GameObject whiteKeyPrefab; 
    public Transform keyPanel;
    public int totalKeys = 10; 

    private float keyWidth = 15.0f; 

    void Start()
    {
        SpawnWhiteKeys();
    }

    void SpawnWhiteKeys()
    {
        foreach (Transform child in keyPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < totalKeys; i++)
        {
            GameObject key = Instantiate(whiteKeyPrefab, keyPanel);
            key.transform.localPosition = new Vector3(i * keyWidth, 0, 0);
        }
    }
}
