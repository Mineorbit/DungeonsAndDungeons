using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target;
    Quaternion targetRotation;
    float rotX = 0;
    float Speed = 2;


    public Vector3 forwardDirection;

    public bool mousefree = true;
    // Start is called before the first frame update
    void Start()
    {
    }
    public bool lockMouse()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        return false;
    }
    public bool unlockMouse()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        return true;
    }


    public void FixedUpdate()
    {
        targetRotation = Quaternion.AngleAxis(rotX,Vector3.up);

        rotX += Speed*Input.GetAxis("Mouse X");

        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Test");
           mousefree = mousefree?lockMouse():unlockMouse();
        }
    }
    void OnDestroy()
    {
        mousefree = unlockMouse();
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,0.125f);
        transform.position = Vector3.Lerp(transform.position,target.transform.position,0.5f);
        PlayerController.forwardDirection = transform.forward;
        PlayerController.rightDirection = transform.right;
    }
}
