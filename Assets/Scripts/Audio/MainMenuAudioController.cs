using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.mineorbit.dungeonsanddungeonscommon;

    public class MainMenuAudioController : AudioController
    {
        void Start()
        {
            Button[] components = Resources.FindObjectsOfTypeAll<Button>();
            foreach (Button b in components)
            {
                b.onClick.AddListener(Select);
            }
            Blend(0, 0);
        }
        void Select()
        {
            Play(0);
        }
    }