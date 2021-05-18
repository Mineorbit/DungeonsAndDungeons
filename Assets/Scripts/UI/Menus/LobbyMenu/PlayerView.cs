using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public static PlayerView playerView;
    public PlayerElement[] playerElements;

    private void Start()
    {
        if (playerView != null) Destroy(this);
        playerView = this;


        playerElements = new PlayerElement[4];
        for (var i = 0; i < playerElements.Length; i++)
            playerElements[i] = transform.Find(i.ToString()).GetComponent<PlayerElement>();
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