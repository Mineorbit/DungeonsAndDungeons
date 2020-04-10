using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectBar : MonoBehaviour
{
    public Button connectButton;
    public InputField name;
    void Start()
    {
        connectButton = transform.GetComponentInChildren<Button>();
        name = transform.GetComponentInChildren<InputField>();
        connectButton.onClick.AddListener(Connect);
    }
    void Connect()
    {
        NetworkManager.current.ConnectToMain(name.text);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
