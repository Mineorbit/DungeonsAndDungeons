using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinMenu : MonoBehaviour
{
    Button lobby;
    Button mainMenu;
    void Start()
    {
        GameObject m = transform.Find("MainMenu").gameObject;
        GameObject l = transform.Find("Lobby").gameObject;
        mainMenu = m.GetComponent<Button>();
        lobby = l.GetComponent<Button>();
        lobby.onClick.AddListener(GameManager.current.startMainMenuMode);
    }

    void Update()
    {
        
    }
}
