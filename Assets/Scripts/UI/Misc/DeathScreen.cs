using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Diagnostics;

public class DeathScreen : Openable
{
    UIAnimation animationScreen;
    UIAnimation animationInfoText;
    //transform of the info text
    public Transform content;
    public TextMeshProUGUI infoTextField;
    public static DeathScreen instance;
    public UnityEvent openEvent;
    //Helps with blocking UI
    GraphicRaycaster rc;
 

    void Start()
    {
        if (instance != null) Destroy(this);
        instance = this;
        content = this.transform.Find("Screen").Find("Content");
        rc = transform.GetComponent<GraphicRaycaster>();

        UnityEvent screenOpenedEvent = new UnityEvent();
        UnityEvent screenClosedEvent = new UnityEvent();
        UnityEvent contentOpenedEvent = new UnityEvent();
        UnityEvent contentClosedEvent = new UnityEvent();


        screenOpenedEvent.AddListener(OpenContent);
        contentOpenedEvent.AddListener(FinishOpen);
        contentClosedEvent.AddListener(CloseScreen);
        screenClosedEvent.AddListener(FinishClose);

        animationScreen = new Fade();
        animationScreen.target = this.transform;
        animationInfoText = new Fade();
        animationInfoText.target = content;
        animationScreen.InEndedEvent = screenOpenedEvent;
        animationScreen.OutEndedEvent = screenClosedEvent;
        animationInfoText.InEndedEvent = contentOpenedEvent;
        animationInfoText.OutEndedEvent = contentClosedEvent;


    }
    void OpenContent()
    {
    }
    void CloseScreen()
    {
        animationScreen.Play();
    }
    void FinishOpen()
    {
        openEvent.Invoke();

        Finished = true;
    }
    void FinishClose()
    {
        Finished = true;
    }
    public override void Open()
    {
        base.Open();
    }
    public override void OnOpen()
    {
        animationScreen.Play();
    }
    public override void OnClose()
    {
        animationInfoText.Play();
    }


}
