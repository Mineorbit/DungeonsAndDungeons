using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    UnityEvent actionEvent;
    public void Awake()
    {

    }
    public void Action()
    {
        actionEvent.Invoke();
    }
}
