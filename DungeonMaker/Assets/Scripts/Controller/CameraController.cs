using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;
    bool followY = true;
    Vector3 targetP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetP = target.position;
        if (!followY) targetP.y = -10;

        transform.position = Vector3.Lerp(transform.position,targetP,0.25f);
    }
}
