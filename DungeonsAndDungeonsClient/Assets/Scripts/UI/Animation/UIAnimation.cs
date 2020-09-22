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
    public bool setup = false;
    public void Setup()
    {
        if (setup) return;
        if(CoroutineManager.instance==null)
        {
            UnityEngine.Debug.Log("Kein CoroutineManager verfügbar, UIAnimation nicht möglich");
        }
        if (targetTransform == null) targetTransform = target.GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = target.GetComponent<CanvasGroup>();
        setup = true;
    }
    public virtual bool Play()
    {
        Setup();
        if (animationPlaying) return false;
        animationPlaying = true;
        return true;
    }
    public virtual void Open()
    {
        Setup();
        canvasGroup.interactable = true;
        open = true;
        InEnded();
    }
    public void InEnded()
    {
        canvasGroup.interactable = true;
        if (InEndedEvent != null)
        {
            InEndedEvent.Invoke();
        }
    }
    public void OutEnded()
    {
        canvasGroup.interactable = false;
        if (OutEndedEvent != null)
        {
            OutEndedEvent.Invoke();
        }
    }

}
