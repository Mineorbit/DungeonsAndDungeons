using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsMenu : Openable
{
    UIAnimation transition;

    public static OptionsMenu options;

    Button backButton;

    TMP_Dropdown resolutionSelector;

    public Toggle fullScreenToggle;

    public Toggle simpleLobbyToggle;
    public GameObject[] playerStores;

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
        options.playerStores = new GameObject[4];


        simpleLobbyToggle = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Lobby").Find("SimpleLobby").GetComponent<Toggle>();
        simpleLobbyToggle.onValueChanged.AddListener(delegate {
            SimpleChange();
        });
        int storedVal = PlayerPrefs.GetInt("SimpleLobby", -1);
        if(storedVal != 0)
            simpleLobbyToggle.isOn = true;
        else
            simpleLobbyToggle.isOn = false;

        SimpleChange();
    }
    void SetupFull()
    {
        fullScreenToggle = transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Disp").Find("Full").GetComponent<Toggle>();
        fullScreenToggle.onValueChanged.AddListener(delegate {
            FullChange();
        });
        int storedVal = PlayerPrefs.GetInt("FullScreen", -1);
        if (storedVal != 0)
            fullScreenToggle.isOn = true;
        else
            fullScreenToggle.isOn = false;
        FullChange();
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
        if (options.playerStores == null)
            options.playerStores = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            if(options.playerStores[i] == null)
            options.playerStores[i] = GameObject.Find("PlayerStore" + i);
        }
        bool set = OptionsMenu.options.simpleLobbyToggle.isOn;
        Debug.Log(set);
        for (int i = 0; i < 4; i++)
        {
            if (options.playerStores[i] != null)
                options.playerStores[i].SetActive(!set);
        }
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
        bool set = fullScreenToggle.isOn;
        Screen.fullScreen = set;
        PlayerPrefs.SetInt("FullScreen", set ? 1 : 0);
        PlayerPrefs.Save();
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
