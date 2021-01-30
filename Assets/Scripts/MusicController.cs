using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : AudioController
{

    void Start()
    {
        Blend(0, 0.125f);
        Play(0);
    }

    
}
