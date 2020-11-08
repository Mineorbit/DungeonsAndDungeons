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
        UnityEvent infoTextClosedEvent = new UnityEvent();

        screenOpenedEvent.AddListener(updateInfoText);

        animationScreen = new Fade();
        animationScreen.target = this.transform;
        animationScreen.InEndedEvent = screenOpenedEvent;

        animationInfoText = new FadeAndGrow();
        animationInfoText.target = text;
        animationInfoText.OutEndedEvent = infoTextClosedEvent;

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
    public override void OnOpen()
    {

        //animationScreen.Play();
        Finished = true;
        openEvent.Invoke();
    }
    public override void OnClose()
    {   
        //animationScreen.Play();
        Finished = true;
    }
    
   
}
