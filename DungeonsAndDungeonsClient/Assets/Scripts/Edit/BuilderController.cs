using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    float speed = 4;
    public static Vector3 builderPosition;
    public static Vector3 builderForward;

    public bool acceptInput = true;

    LevelObjectData selectedObjectType;
    void Start()
    {
        
    }

    void Update()
    {
        UpdateLock();
        if(acceptInput)
        {
        UpdatePosition();
        UpdateRotation();
        ProcessBuildInput();
        }
        builderForward = transform.TransformDirection(Vector3.forward);
    }
    void UpdateLock()
    {
        //Hier gleicher knopf wie bei mouse lock
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            acceptInput = MouseStateController.isLocked();
        }
    }
    void ProcessBuildInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Place();
        }
        if(Input.GetMouseButtonDown(1))
        {
            Displace();
        }
    }
    void Place()
    {
        var t = BuilderCursor.Get();
        if(t.legal)
        Level.currentLevel.Add(t.levelObjectData,t.pos);
    }
    void Displace()
    {
        LevelObject o = BuilderCursor.GetObjectAt();
        Level.currentLevel.Remove(o);
    }
    void UpdatePosition()
    {
        Vector3 targetDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") + transform.up * (Input.GetKey("space") ? 1 : 0) + -transform.up * (Input.GetKey("left shift") ? 1 : 0);
        transform.position += Time.deltaTime * Vector3.Normalize(targetDirection) * speed;
        builderPosition = transform.position;
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
    
}
