using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    float speed = 4;
    float maxDistance = 200;
    void Start()
    {
        
    }

    void Update()
    {
        UpdatePosition();
        UpdateRotation();
        ComputeCursorPosition();
        ProcessBuildInput();
    }
    void ProcessBuildInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Klick");
        }
    }
    void UpdatePosition()
    {
        Vector3 targetDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") + transform.up * (Input.GetKey("space") ? 1 : 0) + -transform.up * (Input.GetKey("left shift") ? 1 : 0);
        transform.position += Time.deltaTime * Vector3.Normalize(targetDirection) * speed;
    }
    void UpdateRotation()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.y += Input.GetAxis("Mouse X");
        float oldX = currentRotation.x;
        currentRotation.x -= Input.GetAxis("Mouse Y");
        if ( !( (300 <= currentRotation.x && currentRotation.x <= 361) || (-1 <= currentRotation.x && currentRotation.x <= 60)))
            currentRotation.x = oldX;
        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }
    void ComputeCursorPosition()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 targetLocation;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            targetLocation = transform.position + transform.TransformDirection(Vector3.forward) * hit.distance;
        }
        else
        {
            targetLocation = transform.position + transform.TransformDirection(Vector3.forward) * maxDistance;
        }
        BuilderCursor.Set(targetLocation,hit.normal);
    }
}
