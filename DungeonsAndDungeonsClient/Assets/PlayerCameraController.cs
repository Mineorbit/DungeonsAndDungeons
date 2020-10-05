using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCameraController : MonoBehaviour
{
    static CinemachineFreeLook cam;
    public void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }
    public static void Setup()
    {
        GameObject target = GameObject.Find("Player");
        cam.LookAt = target.transform;
        cam.Follow = target.transform;
    }
}
