using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateController : MonoBehaviour
{
    static bool locked = false;
    public static bool[] toggleable = new bool[2];
    public void Awake()
    {
        toggleable[0] = true;
        toggleable[1] = true;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)&&toggleable[0]&&toggleable[1])
        {
            if (locked) { Unlock(); } else { Lock(); }
        }
    }
    public static void LockBlocking()
    {
        if (!toggleable[1]) return;
        toggleable[0] = false;
        Lock();
    }
    public static void UnlockUnblocking()
    {
        if (!toggleable[1]) return;
        toggleable[0] = true;
        Unlock();
    }
    public static void LockUnblocking()
    {
        if (!toggleable[0]) return;
        toggleable[1] = true;
        Lock();
    }
    public static void UnlockBlocking()
    {
        if (!toggleable[0]) return;
        toggleable[1] = false;
        Unlock();
    }
    public static void Lock()
    {
        locked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void Unlock()
    {
        locked = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
