using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Diagnostics;

public class LoadingScreen : MonoBehaviour
{
    UIAnimation animationScreen;
    UIAnimation animationInfoText;
    //transform of the info text
    public Transform text;
    public TextMeshProUGUI infoTextField;
    public static LoadingScreen instance;
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
        infoTextClosedEvent.AddListener(closeScreen);

        animationScreen = new Fade();
        animationScreen.target = this.transform;
        animationScreen.InEndedEvent = screenOpenedEvent;

        animationInfoText = new FadeAndGrow();
        animationInfoText.target = text;
        animationInfoText.OutEndedEvent = infoTextClosedEvent;

    }

    public void openLoadingScreen()
    {
        rc.enabled = true;
        animationScreen.Play();
    }
    public void openLoadingScreen(UnityEvent screenOpenedEvent)
    {
        rc.enabled = true;
        animationInfoText.InEndedEvent = screenOpenedEvent;
        animationScreen.Play();
    }

    public void closeLoadingScreen()
    {
        rc.enabled = false;
        animationInfoText.Play();
    }
    public void setLoadingScreenOpen()
    {
        rc.enabled = true;
        setInfoText();
        animationScreen.Open();
        animationInfoText.Open();
    }
    public void setLoadingScreenOpen(UnityEvent screenOpenedEvent)
    {
        rc.enabled = true;
        animationInfoText.InEndedEvent = screenOpenedEvent;
        setInfoText();
        animationScreen.Open();
        animationInfoText.Open();
    }

    public bool isOpen()
    {
        return rc.enabled;
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
    public void closeScreen()
    {
        animationScreen.Play();
    }
    
   
}
