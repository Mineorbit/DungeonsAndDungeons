using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Options : Openable
{
    UIAnimation transition;

    public static Options options;

    Button backButton;

    TMP_Dropdown resolutionSelector;

    Toggle fullScreenToggle;

    Toggle simpleLobbyToggle;

    public void Start()
    {


        if (options != null) Destroy(this);
        options = this;




        backButton = transform.Find("Scroll View").Find("Back").GetComponent<Button>();

        SetupRes();
        SetupFull();
        SetupSimple();

        backButton.onClick.AddListener(Close);
        transition = new Fade();
        transition.target = this.transform;

        res = new Vector2(Screen.width,Screen.height);

    }

    void SetupSimple()
    {
        fullScreenToggle = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Lobby").Find("SimpleLobby").GetComponent<Toggle>();
        fullScreenToggle.onValueChanged.AddListener(delegate {
            SimpleChange();
        });
        int storedVal = PlayerPrefs.GetInt("SimpleLobby", -1);
        if(storedVal != 0)
        fullScreenToggle.isOn = true;
        else
        fullScreenToggle.isOn = false;

        SimpleChange();
    }

    void SimpleChange()
    {
        bool set = fullScreenToggle.isOn;
        HandleSimpleLobbyChange();
        PlayerPrefs.SetInt("SimpleLobby", set ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static void HandleSimpleLobbyChange()
    {
        bool set = Options.options.fullScreenToggle.isOn;
        for (int i = 0; i < 4; i++)
        {
            if (GameObject.Find("PlayerStore" + i) != null)
                GameObject.Find("PlayerStore" + i).SetActive(!set);
        }
    }
    void SetupFull()
    {
        fullScreenToggle = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Disp").Find("Full").GetComponent<Toggle>();
        fullScreenToggle.onValueChanged.AddListener(delegate {
            FullChange();
        });
    }

    void SetupRes()
    {
        resolutionSelector = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Disp").Find("Resolutions").GetComponent<TMP_Dropdown>();

        resolutionSelector.ClearOptions();
        int i = 0;
        string cur = Screen.currentResolution.ToString();
        foreach (Resolution r in Screen.resolutions)
        {


            TMP_Dropdown.OptionData t = new TMP_Dropdown.OptionData();
            t.text = r.ToString();
            resolutionSelector.options.Insert(i,t);
            
            if(r.ToString().Equals(cur))
            {
                resolutionSelector.value = i;
            }

            i++;
        }
        resolutionSelector.onValueChanged.AddListener(delegate {
            ResolutionChange();
        });
    }

    Vector2 res;

    void Update()
    {
        if(res != new Vector2(Screen.width, Screen.height))
        {
            HandleResChange();
            res = new Vector2(Screen.width, Screen.height);
        }
    }

    void FullChange()
    {

        Screen.fullScreen = fullScreenToggle.isOn;

    }

    void HandleResChange()
    {
        LevelList.UpdateDisplay();
    }

    void ResolutionChange()
    {
        int r = resolutionSelector.value;
        if (r > Screen.resolutions.Length) return;
        Screen.SetResolution(Screen.resolutions[r].width, Screen.resolutions[r].height,true);

    }

    public override void OnOpen()
    {
        Debug.Log("Open");
        transition.Play();
        Finished = true;
    }
    public override void OnClose()
    {

        Debug.Log("Close");
        transition.Play();
        Finished = true;
        
    }
}
