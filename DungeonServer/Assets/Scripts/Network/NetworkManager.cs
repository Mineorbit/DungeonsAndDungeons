using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

}
