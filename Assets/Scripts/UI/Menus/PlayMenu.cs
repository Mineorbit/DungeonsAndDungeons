using UnityEngine.UI;

public class PlayMenu : MenuPage
{
    public void Awake()
    {
        base.Awake();
        index = 1;
    }

    public override void Start()
    {
        base.Start();
        Setup();
    }

    public void Setup()
    {
        var lobbyButton = transform.Find("Connect").Find("Go").GetComponent<Button>();
        lobbyButton.onClick.AddListener(goLobby);
    }

    private void goLobby()
    {
        var name = transform.Find("Connect").Find("Name").GetComponentInChildren<InputField>().text;
        var ip = transform.Find("Connect").Find("IP").GetComponentInChildren<InputField>().text;
        LobbyLogic.lobbyLogic.Open(ip, name);
    }
}