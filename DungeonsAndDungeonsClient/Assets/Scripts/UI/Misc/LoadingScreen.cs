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
                          "Der Plural von Wischmops ist Wischmöpse",
                          "Vielleicht gewinnst du etwas wenn du die Bestzeit schlägst",
                          "Wenn du einen Gegner schlägst, der die gleiche Farbe wie du hat, verursachst du mehr Schaden",
                          "Eier und Salz, Milch und Mehl",
                          "Auf, auf und davon!",
                          "Vorwärts immer, rückwärts nimmer",
                          "Nach meiner Kenntnis ist das sofort",
                          "Niemand hat die Absicht einen Dungeon zu errichten",
                          "Kein Walkout",
                          "So ein geiles Produkt",
                          "Wer das liest, niest",
                          "Wer anderen eine Bratwurst brät, der hat ein Bratwurstbratgerät",
                          "Immer wenn ich traurig bin, trink ich einen ...",
                          "Ba-Ba-Banküberfall",
                          "Für Garderobe keine Haftung",
                          "Gut gebügelt ist halb genäht",
                          "Jede Farbe kann besonders gut mit einem Element umgehen",
                          "Wir sind die wahren Spezialisten, unsere Spezialausrüstung ist dort in den Kisten",
                          "Wir produzieren Fleisch - Fleisch, Fleisch, Fleisch"};


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


        screenOpenedEvent.AddListener(OpenText);
        textOpenedEvent.AddListener(FinishOpen);
        textClosedEvent.AddListener(CloseScreen);
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
    void OpenText()
    {
        updateInfoText();
        animationInfoText.Play();

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
