using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform barTransform;
    private float currentT;
    private Vector2 defaultPosition;
    private float T;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        barTransform = transform.Find("Bar").GetComponent<RectTransform>();
        defaultPosition = barTransform.position;
        T = 1f;
    }

    private void Update()
    {
        var t = PlayerManager.playerManager.players[PlayerManager.currentPlayerLocalId].health / 100f;
        SetTarget(t);
        currentT = (currentT + T) / 2;
        SetFillState(currentT);
    }


    private void SetFillState(float t)
    {
        barTransform.position = defaultPosition + new Vector2(-Screen.width * 0.25f * (1 - t), 0);
    }

    public void SetTarget(float t)
    {
        T = t;
    }
}