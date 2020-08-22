using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class FadeAndGrow : UIAnimation
{
    bool open = false;

    public override bool Play()
    {
        if(!base.Play()) return false;
        if (open)
        {
            open = false;

            CoroutineManager.instance.StartCoroutine(FadeOut());
        }else
        {
            open = true;

            CoroutineManager.instance.StartCoroutine(FadeIn());
        }
        return true;
    }
    public IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1.05; i += 0.1f)
        {
            canvasGroup.alpha = i;
            target.localScale = new Vector3(i,i,i);
            yield return null;
        }
        animationPlaying = false;
    }

    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= -0.05; i -= 0.1f)
        {
            canvasGroup.alpha = i;
            target.localScale = new Vector3(i, i, i);
            yield return null;
        }
        animationPlaying = false;
    }
}
