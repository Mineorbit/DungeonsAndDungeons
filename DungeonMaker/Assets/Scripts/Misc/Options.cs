using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    Button backButton;
    TMP_Dropdown resolutionList;
    Slider masterAudio;


    int currentN;
    bool changeable = true;

    public int numberOfMixers = 2;
    public float[] Volume;
    public AudioMixer[] mixers= new AudioMixer[2];
    

    public void Setup()
    {
        Volume = new float[numberOfMixers];
        backButton=transform.Find("Close").GetComponent<Button>();
        masterAudio = transform.Find("Volume").GetComponent<Slider>();
        resolutionList = transform.Find("Resolutions").GetComponent<TMP_Dropdown>();
        backButton.onClick.AddListener(close);
        setupResolutionSetter();
        setupAudioControllers();


        
    }
    public float correctForMain(float v){
        return 100*v-80;
    }
    public void setupAudioControllers()
    {
        for(int i = 0;i<numberOfMixers;i++)
        {    
        Volume[i] = PlayerPrefs.GetFloat("Volume"+i);
        masterAudio.value = Volume[i];
        if(mixers[i]!=null)
        mixers[i].SetFloat("Volume",correctForMain(Volume[i]));
        }
    }
    public void setMainValue(float v)
    {
        Volume[0] = v;
        PlayerPrefs.SetFloat("Volume"+0,v);
        mixers[0].SetFloat("Volume",correctForMain(v));
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
    }
    
}
