using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetInterface : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    void Start()
    {
        
    }
    public void OnEnable()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDisable()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
