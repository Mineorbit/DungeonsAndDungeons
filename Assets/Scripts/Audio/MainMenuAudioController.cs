using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudioController : AudioController
{
    private void Start()
    {
        var components = Resources.FindObjectsOfTypeAll<Button>();
        foreach (var b in components) b.onClick.AddListener(Select);
        Blend(0, 0);
    }

    private void Select()
    {
        Play(0);
    }
}