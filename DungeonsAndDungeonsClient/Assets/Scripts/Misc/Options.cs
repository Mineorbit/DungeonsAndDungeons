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

    public void Start()
    {
        if (options != null) Destroy(this);
        options = this;

        backButton = transform.Find("Scroll View").Find("Back").GetComponent<Button>();
        backButton.onClick.AddListener(Close);
        transition = new Fade();
        transition.target = this.transform;
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
