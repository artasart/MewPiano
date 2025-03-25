using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EasingFunction;

public class ButtonClickAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Ease easeFunction = Ease.EaseOutBack;
    public float targetScaleY = .75f;

    private Button button;
    
    public Action action_PointerDown;
    public Action action_PointerUp;

    CoroutineHandle handle_Button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Timing.KillCoroutines(handle_Button);

        handle_Button = Timing.RunCoroutine(Co_SetCanvasGroupAlpha(targetScaleY));

        action_PointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Timing.KillCoroutines(handle_Button);

        handle_Button = Timing.RunCoroutine(Co_SetCanvasGroupAlpha(1f));

        action_PointerUp?.Invoke();
    }

    private IEnumerator<float> Co_SetCanvasGroupAlpha(float _alpha)
    {
        var canvasgroup = button.GetComponent<CanvasGroup>();
        var current = canvasgroup.alpha;

        float lerpvalue = 0f;

        while (lerpvalue <= 1f)
        {
            Function function = GetEasingFunction(easeFunction);

            float x = function(current, _alpha, lerpvalue);

            lerpvalue += 3f * Time.deltaTime;

            canvasgroup.alpha = x;

            yield return Timing.WaitForOneFrame;
        }

        button.GetComponent<CanvasGroup>().alpha = _alpha;
    }
}
