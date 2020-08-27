using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    UIAnimation transferAnimation;
    public virtual void Start()
    {
        setupUI();
    }
    void setupUI()
    {
        transferAnimation = new Fade();
        transferAnimation.target = this.transform;
    }

}
