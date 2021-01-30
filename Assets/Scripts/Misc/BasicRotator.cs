using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0.25f,0.5f,1,0);
    }
}
