using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public int index;
    UIAnimation transferAnimation;
    public virtual void Awake()
    {
        setupTransition();
    }
    public virtual void Start()
    {
    }
    public void setupTransition()
    {
        transferAnimation = new Fade();
        transferAnimation.target = this.transform;
    }
    public virtual void Open()
    {
        transferAnimation.Play();
    }
    public virtual void Close()
    {
        transferAnimation.Play();
    }

}
