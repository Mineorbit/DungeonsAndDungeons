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
        SetTarget(0);
    }
    public static void SetTarget(int localId)
    {
        if (localId > 3 || localId < 0) return;

        Transform target = PlayerManager.playerControllers[localId].transform;
        cam.LookAt = target;
        cam.Follow = target;
    }
}
