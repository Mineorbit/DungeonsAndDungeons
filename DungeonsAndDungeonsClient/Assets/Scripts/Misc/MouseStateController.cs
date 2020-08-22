using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateController : MonoBehaviour
{
    bool locked = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            locked = !locked;
        }
    }
}
