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

        nameField.text = playerData.name;
        
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