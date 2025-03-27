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

        // Set the button's color based on its index (use alpha value)
        Image buttonImage = test.GetComponent<Image>();
        if (buttonImage != null)
        {
            // Set the color based on the index, for example, varying the alpha value
            float alphaValue = Mathf.Clamp01(index / (float)pianoCount); // Normalize index to [0,1]
            buttonImage.color = new Color(1f, 1f, 1f, alphaValue); // R, G, B = 1 (white), A = alpha
        }

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
