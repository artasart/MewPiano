using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Last : Panel_Base
{
    public GameObject pianoObject;
    public Transform pianoParent;

    protected override void Awake()
    {
        base.Awake();
    }

    public int pianoCount = 44;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DestroyChild();

            for (int i = 0; i < pianoCount; i++)
            {
                MakePiano(i);
            }
        }
    }

    public void MakePiano(int index)
    {
        var test = Instantiate(pianoObject);
        test.transform.SetParent(pianoParent, false);  // Using SetParent with false to maintain local position and scale
        test.transform.localPosition = Vector3.zero;
        test.transform.localScale = Vector3.one;

        

        // Add the onClick listener
        test.GetComponent<Button>().onClick.AddListener(() => Onclick_Piano(index));
    }

    private void Onclick_Piano(int index)
    {
        Debug.Log($"{index} 번째 눌림");
    }

    private void DestroyChild()
    {
        foreach (Transform item in pianoParent)
        {
            Destroy(item.gameObject);
        }
    }
}
