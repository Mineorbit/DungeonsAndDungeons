using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Diagnostics;

public class LoadingScreen : Openable
{
    UIAnimation animationScreen;
    UIAnimation animationInfoText;
    //transform of the info text
    public Transform text;
    public TextMeshProUGUI infoTextField;
    public static LoadingScreen instance;
    public UnityEvent openEvent;
    //Helps with blocking UI
    GraphicRaycaster rc;
    string[] infoText = { "Benutze W/A/S/D zum laufen",
                          "Manche Dungeons sind nur zu viert schaffbar",
                          "Wenn du mal nicht weiter kommst, kannst du jeder Zeit ohne Strafe die Runde abbrechen",
                          "Der Plural von Wischmops ist Wischmöpse"};


    void Start()
    {
        if (instance != null) Destroy(this);
        instance = this;
        text = this.transform.Find("Screen").Find("TextInfo");
        infoTextField = text.GetComponent<TextMeshProUGUI>();
        rc = transform.GetComponent<GraphicRaycaster>();

        UnityEvent screenOpenedEvent = new UnityEvent();
        UnityEvent screenClosedEvent = new UnityEvent();
        UnityEvent textOpenedEvent = new UnityEvent();
        UnityEvent textClosedEvent = new UnityEvent();


        screenOpenedEvent.AddListener(updateInfoText);
        screenOpenedEvent.AddListener(FinishOpen);

        screenClosedEvent.AddListener(FinishClose);
        
        animationScreen = new Fade();
        animationScreen.target = this.transform;
        animationInfoText = new Fade();
        animationInfoText.target = text;
        animationScreen.InEndedEvent = screenOpenedEvent;
        animationScreen.OutEndedEvent = screenClosedEvent;
        animationInfoText.InEndedEvent = textOpenedEvent;
        animationInfoText.OutEndedEvent = textClosedEvent;


    }
    void FinishOpen()
    {
        UnityEngine.Debug.LogError("Open Finished");
        openEvent.Invoke();
        Finished = true;
    }
    void FinishClose()
    {
        Finished = true;
    }
    public void setInfoText()
    {
        int rnd = UnityEngine.Random.Range(0, infoText.Length - 1);
        infoTextField.SetText(infoText[rnd]);
    }
    public void updateInfoText()
    {
        setInfoText();
        animationInfoText.Play();
    }
    public override void Open()
    {
        UnityEngine.Debug.LogError("Open called");
        base.Open();
    }
    public override void OnOpen()
    {

        UnityEngine.Debug.Log("Opening");
        animationScreen.Play();
    }
    public override void OnClose()
    {
        UnityEngine.Debug.Log("Closing");
        animationScreen.Play();
    }
    
   
}
