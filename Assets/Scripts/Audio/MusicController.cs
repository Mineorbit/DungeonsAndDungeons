using System.Collections.Generic;
using com.mineorbit.dungeonsanddungeonscommon;

public class MusicController : AudioController
{
    private GameManager.State lastState = GameManager.Init;

    private readonly Dictionary<GameManager.State, int> music = new Dictionary<GameManager.State, int>();

    private void Start()
    {
        Blend(0, 1f);
        music.Add(GameManager.Init, 0);
        music.Add(GameManager.MainMenu, 0);
        music.Add(GameManager.Edit, 1);
    }

    private void Update()
    {
        if (GameManager.GetState() != lastState)
        {
            if (music.ContainsKey(lastState) && music.ContainsKey(GameManager.GetState()))
                CrossFade(music[lastState], music[GameManager.GetState()], 2f);
            lastState = GameManager.GetState();
        }
    }
}