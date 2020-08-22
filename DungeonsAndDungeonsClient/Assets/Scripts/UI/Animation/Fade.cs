using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : UIAnimation
{
    bool open = false;

    public override bool Play()
    {
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
        for (float i = 0; i <= 1.05; i += 0.1f)
        {
            canvasGroup.alpha = i;
            yield return null;
        }
        animationPlaying = false;
    }

    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= -0.05; i -= 0.1f)
        {
            canvasGroup.alpha = i;
            yield return null;
        }
        animationPlaying = false;
    }
}
