using Cinemachine;
using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private static CinemachineFreeLook cam;

    public void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    public void Update()
    {
        var localId = PlayerManager.currentPlayerLocalId;
        if (localId < 0 || localId > 3) return;

        Transform target = null;

        if (PlayerManager.playerManager.playerControllers[localId] != null)
            target = PlayerManager.playerManager.playerControllers[localId].transform;

        if (target != null)
        {
            cam.LookAt = target;
            cam.Follow = target;
        }
    }

    public static void Setup()
    {
    }
}