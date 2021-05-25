using com.mineorbit.dungeonsanddungeonscommon;


public class LobbyHandler : OptionHandler
{
    public override void OnValueChanged(object val)
    {
        var simpleLobby = (bool) val;

        PlayerStore.Set(!simpleLobby);
    }
}