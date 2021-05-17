using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
    private TextMeshProUGUI nameField;

    private void Awake()
    {
        nameField = transform.Find("UserName").GetComponent<TextMeshProUGUI>();
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
}