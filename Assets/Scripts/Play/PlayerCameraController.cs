using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using com.mineorbit.dungeonsanddungeonscommon;

public class PlayerCameraController : MonoBehaviour
{
    static CinemachineFreeLook cam;

    public void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }
    public static void Setup()
    {
    }
    public void Update()
    {
        int localId = PlayerManager.currentPlayerLocalId;
        if (localId < 0 || localId > 3) return;

        Transform target = PlayerManager.playerManager.playerControllers[localId].transform;
        cam.LookAt = target;
        cam.Follow = target;
        
    }
}
