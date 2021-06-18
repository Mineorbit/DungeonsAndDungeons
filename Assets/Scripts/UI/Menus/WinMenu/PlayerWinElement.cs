using System.Linq;
using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;

public class PlayerWinElement : MonoBehaviour
{
    private TextMeshProUGUI nameField;
    private RectTransform background;
    public GameObject readyMarker; 
    private void Awake()
    {
        nameField = transform.Find("UserName").GetComponent<TextMeshProUGUI>();
        background = transform.Find("Background").GetComponent<RectTransform>();
    }

    public void UpdateElement(Player playerData)
    {
        if (playerData == null)
        {
            nameField.text = "";
            return;
        }

        nameField.text = playerData.name +$" ({playerData.points})";
        int maxPoints = PlayerManager.playerManager.players.Select((x) => x != null ? x.points : 0).Max();
        Debug.Log("Max Points: "+maxPoints);
        float score = 0;
        if (maxPoints != 0)
        {
            score = (float) playerData.points / (float)maxPoints;
        }
        SetScore(score);
    }

    public void SetScore(float t)
    {
        background.localScale = new Vector3(1, t, 1);
    }

    public void SetReady(bool r)
    {
        MainCaller.Do( () => readyMarker.SetActive(r));
    }
}