using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelObjectDataSelector : MonoBehaviour
{
    public LevelObjectData data;
    Button selectButton;
    public TextMeshProUGUI nameTextField;
    void Awake()
    {
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(Select);
        nameTextField = transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }
    public void SetData(LevelObjectData d)
    {
        data = d;
        nameTextField.SetText(d.FullName);
    }
    void Select()
    {
        Debug.Log("Pressed "+data.FullName);
        BuilderCursor.Set(data);
    }
}
