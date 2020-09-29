using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
    TMPro.TextMeshProUGUI nameField;
    void Awake()
    {
        nameField = transform.Find("UserName").GetComponent<TMPro.TextMeshProUGUI>();
    }
    public void UpdateElement(Player playerData)
    {
        if(playerData == null)
        {
            nameField.text = "";
            return;
        }

        nameField.text = playerData.name;
    }
}
