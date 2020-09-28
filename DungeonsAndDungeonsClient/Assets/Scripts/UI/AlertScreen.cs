using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertScreen : MonoBehaviour
{
    public static AlertScreen alert;
    bool open;
    UIAnimation animation;
    void Start()
    {
        if (alert != null) Destroy(alert);
        alert = this;
    }

    public void Open()
    {
        if (open) return;
        open = true;
        BlurScreen.blurScreen.Open();
    }
    public void Close()
    {
        if (!open) return;
        open = false;
        BlurScreen.blurScreen.Close();
    }
}
