using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
public class UIAnimation
{
    public Transform target;
    public RectTransform targetTransform;
    public CanvasGroup canvasGroup;
    public UnityEvent InEndedEvent;
    public UnityEvent OutEndedEvent;
    public bool animationPlaying = false;
    public bool open = false;
    public virtual bool Play()
    {
        if (targetTransform == null) targetTransform = target.GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = target.GetComponent<CanvasGroup>();
        if (animationPlaying) return false;
        animationPlaying = true;
        return true;
    }
    public virtual void Open()
    {
        if (targetTransform == null) targetTransform = target.GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = target.GetComponent<CanvasGroup>();
        
        open = true;
        InEnded();
    }
    public void InEnded()
    {
        if (InEndedEvent != null)
        {
            InEndedEvent.Invoke();
        }
    }
    public void OutEnded()
    {
        if (OutEndedEvent != null)
        {
            OutEndedEvent.Invoke();
        }
    }

}
