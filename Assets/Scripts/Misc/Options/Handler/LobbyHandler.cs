using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : OptionHandler
{

    public override void OnValueChanged(object val)
    {
        bool simpleLobby = (bool) val;

        PlayerStore.Set(!simpleLobby);
    }
}
