using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlertScreen : MonoBehaviour
{
    public static AlertScreen alert;
    bool open;
    UIAnimation animation;
    Button closeButton;

    TMPro.TextMeshProUGUI text;
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
        text = transform.Find("Sub").Find("Cover").Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Reset()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Close);
    }
    public void Open()
    {
        if (open) return;
        open = true;

        Reset();
        BlurScreen.blurScreen.Open();
        animation.Open();
    }
    public void Open(bool closeManual)
    {

    }
    public void Open(bool closeManual, string text)
    {

    }
    public void Open(string t)
    {
        Open();
        text.text = t;
    }
    public void Open(string t, UnityEvent cancelEvent)
    {
        Open();
        closeButton.onClick.AddListener(cancelEvent.Invoke);
        text.text = t;
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
