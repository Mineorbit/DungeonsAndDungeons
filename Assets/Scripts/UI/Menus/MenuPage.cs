using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : Openable
{
    public int index;
    UIAnimation transferAnimation;
    public override void Awake()
    {
        setupTransition();
        base.Awake();
    }
    public virtual void Start()
    {
    }
    public void setupTransition()
    {
        transferAnimation = new Fade();
        transferAnimation.target = this.transform;
    }
    public override void OnOpen()
    {
        Debug.Log("Playing animation: "+this);
        transferAnimation.Play();
        Finished = true;
    }
    public override void OnClose()
    {
        transferAnimation.Play();
        Finished = true;
    }

}
