using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{

    // Start is called before the first frame update
    void Start()
    {
        rotationHand = Quaternion.Euler(0f,90f,90f);
        offsetHand = new Vector3(0.1f,0f,0f);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    
}
