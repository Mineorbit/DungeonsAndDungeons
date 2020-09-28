using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertScreen : MonoBehaviour
{
    public static AlertScreen alert;
    bool open;
    UIAnimation animation;
    Button closeButton;
    void Start()
    {
        if (alert != null) Destroy(alert);
        alert = this;
        SetupUI();
    }
    void SetupUI()
    {
        animation = new Fade();
        animation.target = this.transform;
        closeButton = transform.Find("Sub").Find("Button").GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }

    public void Open()
    {
        if (open) return;
        open = true;
        BlurScreen.blurScreen.Open();
        animation.Open();
    }
    public void Open(bool closeManual)
    {

    }
    public void Open(bool closeManual, string text)
    {

    }
    public void Open(string text)
    {

    }
    public void Open(string text, float waitTime)
    {

    }
    public void Close()
    {
        if (!open) return;
        open = false;
        BlurScreen.blurScreen.Close();
        animation.Close();
    }
}
