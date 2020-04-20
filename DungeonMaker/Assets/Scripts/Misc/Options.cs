using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    Button backButton;
    TMP_Dropdown resolutionList;
    Slider masterAudio;


    int currentN;
    bool changeable = true;

    public final int numberOfMixers = 2;
    AudioMixer[] mixers = new AudioMixer[numberOfMixers];
    

    public void Setup()
    {
        backButton=transform.Find("Close").GetComponent<Button>();
        resolutionList = transform.Find("Resolutions").GetComponent<TMP_Dropdown>();
        backButton.onClick.AddListener(close);
        setupResolutionSetter();
        setupAudioControllers();


        
    }
    public void setupAudioControllers()
    {

    }

    public void setupResolutionSetter()
    {
        changeable = false;
        resolutionList.ClearOptions();
        List<string> options = new List<string>();
        int index = 0;
        int v = 0;
        foreach(Resolution res in Screen.resolutions)
        {
            Resolution r = res;
            options.Add(r.ToString());
            if(r.Equals(Screen.currentResolution)) v = index;
            index++;
        }
        resolutionList.AddOptions(options);
        resolutionList.value = v;
        changeable = true;
    }
    public void handleResolutionData(int n)
    {
        if(changeable)
        {
            
        Resolution r = Screen.resolutions[n];
        currentN = n;
        Screen.SetResolution(r.width,r.height,true,r.refreshRate);
        }
    }
    void  close()
    {
        UIManager.current.closeOption();
    }
    void Update()
    {
        Debug.Log(Screen.currentResolution);
    }
    
}
