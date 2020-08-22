using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    float speed = 4;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetDirection = transform.forward*Input.GetAxis("Vertical") + transform.right*Input.GetAxis("Horizontal") + transform.up * (Input.GetKey("space") ? 1 : 0)+ -transform.up * (Input.GetKey("left shift") ? 1 : 0);
        transform.position += Time.deltaTime * Vector3.Normalize(targetDirection)*speed;
        transform.Rotate(-Input.GetAxis("Mouse Y")*transform.right+Input.GetAxis("Mouse X")*Vector3.up,Space.World);
    }
}
