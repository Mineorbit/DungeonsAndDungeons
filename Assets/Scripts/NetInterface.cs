using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetInterface : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private Button downloadButton;
    void Start()
    {
        SetupNetInterface();
    }

    void SetupNetInterface()
    {
        downloadButton = transform.Find("Download").GetComponent<Button>();
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
