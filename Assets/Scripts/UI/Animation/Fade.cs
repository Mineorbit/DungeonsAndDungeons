using System.Collections;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class Fade : UIAnimation
{
    private readonly float fadeSpeed = 2;

    public override void Open()
    {
        if (!open)
            base.Open();
        else return;
        animationPlaying = false;

        canvasGroup.alpha = 1;
    }

    public override void Close()
    {
        if (open)
            base.Close();
        else return;
        animationPlaying = true;

        canvasGroup.alpha = 0;
    }

    public override bool Play()
    {
        if (!base.Play()) return false;
        if (open)
        {
            open = false;

            MainCaller.startCoroutine(FadeOut());
        }
        else
        {
            open = true;

            MainCaller.startCoroutine(FadeIn());
        }

        return true;
    }

    public IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1.05; i += fadeSpeed * Time.deltaTime)
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