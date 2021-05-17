using com.mineorbit.dungeonsanddungeonscommon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelObjectDataSelector : MonoBehaviour
{
    public LevelObjectData data;
    public TextMeshProUGUI nameTextField;
    private Button selectButton;

    private void Awake()
    {
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(Change);
        nameTextField = transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }

    public void SetData(LevelObjectData d)
    {
        data = d;
        nameTextField.SetText(d.name);
    }

    public void Change()
    {
    }

    public void Select()
    {
        BuilderCursor.Set(data);
    }
}