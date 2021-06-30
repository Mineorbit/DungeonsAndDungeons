using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlertScreen : MonoBehaviour
{
    public static AlertScreen alert;
    private UIAnimation uiAnimation;
    private Button closeButton;
    private bool open;

    private TextMeshProUGUI text;

    public void Reset()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        if (alert != null) Destroy(alert);
        alert = this;
        SetupUI();
    }

    private void SetupUI()
    {
        uiAnimation = new Fade();
        uiAnimation.target = transform;
        closeButton = transform.Find("Sub").Find("Button").GetComponent<Button>();
        text = transform.Find("Sub").Find("Cover").Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void Open()
    {
        if (open) return;
        open = true;

        Reset();
        BlurScreen.blurScreen.Open();
        uiAnimation.Open();
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
        uiAnimation.Close();
    }
}