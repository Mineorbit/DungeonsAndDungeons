using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
public class UIAnimation
{
    public Transform target;
    public RectTransform targetTransform;
    public CanvasGroup canvasGroup;
    public UnityEvent animationEndedEvent;
    public bool animationPlaying = false;
    public virtual bool Play()
    {
        if (targetTransform == null) targetTransform = target.GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = target.GetComponent<CanvasGroup>();
        if (animationPlaying) return false;
        animationPlaying = true;
        return true;
    }
   
}
