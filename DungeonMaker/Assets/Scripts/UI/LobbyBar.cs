using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBar : MonoBehaviour
{
    public static LobbyBar current;
    public CanvasGroup slider;
    GameObject bar;
    public bool opened = false;
    public void Start()
     {
        current = this;
        bar = this.gameObject;
        slider = GetComponent<CanvasGroup>();

    }
    
   
    public void open()
    {

		bar.SetActive(true);
        if(!opened)
        {
        StartCoroutine("FadeIn");
        opened = true;
        }
    }
    public void close()
    {
        Debug.Log("Test");
        if(opened)
        {
        StartCoroutine("FadeOut");
        opened = false;
        }
    }
    IEnumerator FadeOut () {

		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			slider.alpha = ft;
			yield return null;
		}
		slider.alpha = 0;
		bar.SetActive(false);
        slider.interactable = false;
        slider.blocksRaycasts = false;
		yield return null;
	}

	IEnumerator FadeIn () {

		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			slider.alpha = ft;
			yield return null;
		}
		slider.alpha = 1;
        slider.interactable = true;
        slider.blocksRaycasts = true;

		yield return null;
	}
}
