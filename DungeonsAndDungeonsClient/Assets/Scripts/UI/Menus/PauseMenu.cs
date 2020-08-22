using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public UIAnimation animation;
    void Start()
    {
        animation = new FadeAndGrow();
        animation.target = this.transform;
    }

    void Update()
    {
    if(Input.GetKeyDown(KeyCode.Escape))
        {
            animation.Play();
        }
    }
}
