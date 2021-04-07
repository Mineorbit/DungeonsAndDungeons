using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.mineorbit.dungeonsanddungeonscommon;

public class LevelObjectDataSelector : MonoBehaviour
{
    public LevelObjectData data;
    Button selectButton;
    public TextMeshProUGUI nameTextField;
    void Awake()
    {
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(Change);
        nameTextField = transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }
    public void SetData(LevelObjectData d)
    {
        data = d;
        nameTextField.SetText(d.FullName);
    }
    public void Change()
    {

    }
    public void Select()
    {
        Debug.Log("Setting: "+data);
        BuilderCursor.Set(data);
    }
}
