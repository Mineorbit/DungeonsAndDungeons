using UnityEngine;
using UnityEngine.UI;

public class TestHUD : MonoBehaviour
{
    public Button enterEdit;

    private void Start()
    {
        enterEdit.onClick.AddListener(EnterEdit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) enterEdit.onClick.Invoke();
    }

    private void EnterEdit()
    {
        GameManager.instance.performAction(GameManager.EnterEditFromTest);
    }
}