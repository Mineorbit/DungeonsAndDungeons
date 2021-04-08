using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mineorbit.dungeonsanddungeonscommon;

public class Bar : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform barTransform;
    float currentT;
    float T;
    Vector2 defaultPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        barTransform = transform.Find("Bar").GetComponent<RectTransform>();
        defaultPosition = barTransform.position;
        T = 1f;
    }


    void SetFillState(float t)
    {
        barTransform.position = defaultPosition + new Vector2(-Screen.width*0.25f*(1-t),0);
    }

    public void SetTarget(float t)
    {
        T = t;
    }
    void Update()
    {
        float t = (float) PlayerManager.playerManager.players[PlayerManager.currentPlayerLocalId].health / 100f;
        SetTarget(t);
        currentT = (currentT + T) / 2;
        SetFillState(currentT);
    }
}
