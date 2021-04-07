using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class Fade : UIAnimation
{
    float fadeSpeed = 2;
    public override void Open()
    {
        if (CoroutineManager.instance == null) return;
        if (!base.open)
        { base.Open(); }
        else return;
        animationPlaying = false;

        canvasGroup.alpha = 1;
    }
    public override void Close()
    {
        if (CoroutineManager.instance == null) return;
        if (base.open)
        { base.Close(); }
        else return;
        animationPlaying = true;

        canvasGroup.alpha = 0;
    }

    public override bool Play()
    {
        if (CoroutineManager.instance == null) return false;
        if (!base.Play()) return false;
        if (open)
        {
            open = false;

            CoroutineManager.instance.StartCoroutine(FadeOut());
        }
        else
        {
            open = true;

            CoroutineManager.instance.StartCoroutine(FadeIn());
        }
        return true;
    }
    public IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1.05; i += fadeSpeed*Time.deltaTime)
        {
            canvasGroup.alpha = i;
            yield return null;
        }

        canvasGroup.alpha = 1;
        animationPlaying = false;
        InEnded();
     }

    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= -0.05; i -= fadeSpeed * Time.deltaTime)
        {
            canvasGroup.alpha = i;
            yield return null;
        }
        canvasGroup.alpha = 0;
        animationPlaying = false;
        OutEnded();
    }
}
