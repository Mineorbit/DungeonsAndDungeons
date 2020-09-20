using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public int index;
    UIAnimation transferAnimation;
    public virtual void Awake()
    {
        setupUI();
    }
    public virtual void Start()
    {
    }
    void setupUI()
    {
        transferAnimation = new Fade();
        transferAnimation.target = this.transform;
    }
    public void Open()
    {
        Debug.Log(transferAnimation);
        transferAnimation.Play();
    }
    public void Close()
    {
        transferAnimation.Play();
    }

}
