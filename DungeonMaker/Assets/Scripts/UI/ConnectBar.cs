using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectBar : MonoBehaviour
{
    public Button connectButton;
    public InputField nameField;
    void Start()
    {
        connectButton = transform.Find("GO").GetComponentInChildren<Button>();
        nameField = transform.GetComponentInChildren<InputField>();
        connectButton.onClick.AddListener(Connect);
    }
    void Connect()
    {
        Client.instance.name = nameField.text;
        Client.instance.ConnectToMainServer();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
