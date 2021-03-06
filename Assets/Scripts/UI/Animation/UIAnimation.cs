﻿using UnityEngine;
using UnityEngine.Events;

public class UIAnimation
{
    public bool animationPlaying;
    public CanvasGroup canvasGroup;
    public UnityEvent InEndedEvent = new UnityEvent();
    public bool open;
    public UnityEvent OutEndedEvent = new UnityEvent();
    public bool setup;
    public Transform target;
    public RectTransform targetTransform;

    public void Setup()
    {
        if (setup) return;
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

    public bool isOpen()
    {
        return open && !animationPlaying;
    }

    public virtual void Open()
    {
        Setup();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        open = true;
        InEnded();
    }

    public virtual void Close()
    {
        Setup();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        open = false;
        OutEnded();
    }

    public void InEnded()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        if (InEndedEvent != null) InEndedEvent.Invoke();
    }

    public void OutEnded()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (OutEndedEvent != null) OutEndedEvent.Invoke();
    }
}