using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
   public float Speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,Speed*Time.deltaTime,0);
    }
}
