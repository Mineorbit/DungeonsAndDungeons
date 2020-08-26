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
    public int eventCount;
    string[] infoText = { "Benutze W/A/S/D zum laufen",
                          "Manche Dungeons sind nur zu viert schaffbar",
                          "Wenn du mal nicht weiter kommst, kannst du jeder Zeit die Runde abbrechen" };


    void Start()
    {
        text = this.transform.Find("Screen").Find("TextInfo");
        infoTextField = text.GetComponent<TextMeshProUGUI>();
        rc = transform.GetComponent<GraphicRaycaster>();
        animationScreen = new Fade();

        UnityEvent closeEvent = new UnityEvent();
        UnityEvent closeSecEvent = new UnityEvent();

        closeEvent.AddListener(updateInfoText);
        closeSecEvent.AddListener(closeScreen);

        animationScreen.target = this.transform;
        animationScreen.animationEndedEvent = closeEvent;
        animationInfoText = new FadeAndGrow();
        animationInfoText.target = text;
        animationInfoText.animationEndedEvent = closeSecEvent;

        if (instance != null) Destroy(this);
        instance = this;
    }

    public void openLoadScreen()
    {
        rc.enabled = true;
        eventCount = 2;
        pickInfoText();
        animationScreen.Play();
    }
    public void closeLoadScreen()
    {

        rc.enabled = false;
        eventCount = 2;
        animationInfoText.Play();
    }
    public void closeScreen()
    {
        UnityEngine.Debug.Log("Tset");
        if (eventCount == 0)
            return;
        UnityEngine.Debug.Log("Test");
            eventCount--;
        animationScreen.Play();
    }
    public void updateInfoText()
    {
        if (eventCount == 0)
            return;

        eventCount--;
        pickInfoText();
        OpenText();
    }
    void pickInfoText()
    {
        int i = UnityEngine.Random.Range(0,infoText.Length-1);
        infoTextField.SetText(infoText[i]);
    }
    public void OpenText()
    {
        animationInfoText.Play();
    }
   
}
