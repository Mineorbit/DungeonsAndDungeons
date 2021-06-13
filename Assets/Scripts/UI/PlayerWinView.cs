using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class PlayerWinView : MonoBehaviour
{
    public static PlayerWinView playerWinView;
    public PlayerWinElement[] playerElements;

    private void Start()
    {
        if (playerWinView != null) Destroy(this);
        playerWinView = this;


        playerElements = new PlayerWinElement[4];
        for (var i = 0; i < playerElements.Length; i++)
            playerElements[i] = transform.Find(i.ToString()).GetComponent<PlayerWinElement>();
        UpdatePlayerView();
    }

    public void Update()
    {
        UpdatePlayerView();
    }


    public void UpdatePlayerView()
    {
        for (var i = 0; i < playerElements.Length; i++)
        {
            bool activate = PlayerManager.playerManager.players[i] != null;
            playerElements[i].gameObject.SetActive(activate);
            if(activate)
                playerElements[i].UpdateElement(PlayerManager.playerManager.players[i]);
        }
    }
}