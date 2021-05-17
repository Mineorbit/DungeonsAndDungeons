using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    public static Vector3 builderPosition;
    public static Vector3 builderForward;

    public bool acceptInput = true;

    public BuilderCursor builderCursor;
    private InteractiveLevelObject receiver;

    private LevelObjectData selectedObjectType;
    private InteractiveLevelObject sender;
    private readonly float speed = 4;
    private Wire w;

    private void Start()
    {
    }

    private void Update()
    {
        UpdateLock();
        if (acceptInput)
        {
            UpdatePosition();
            UpdateRotation();
            ProcessBuildInput();
        }

        builderForward = transform.TransformDirection(Vector3.forward);
    }

    private void UpdateLock()
    {
        //Hier gleicher knopf wie bei mouse lock
        if (Input.GetKeyDown(KeyCode.LeftAlt)) acceptInput = MouseStateController.isLocked();
    }

    private InteractiveLevelObject pickAtCursor()
    {
        var o = builderCursor.GetGameObjectAt();
        return o.GetComponent<InteractiveLevelObject>();
    }

    private void ProcessBuildInput()
    {
        if (Input.GetMouseButtonDown(0)) Place();
        if (Input.GetMouseButtonDown(1)) Displace();

        if (Input.GetMouseButtonDown(2))
        {
            sender = pickAtCursor();
            if (sender != null)
            {
                Debug.Log("Wire started");
                w = Wire.CreateDynamic(sender.transform.position, sender.transform.position, Color.black);
            }
        }
        else if (Input.GetMouseButton(2))
        {
            if (w != null)
            {
                var targetPoint = BuilderCursor.targetPosition;
                receiver = pickAtCursor();
                if (receiver != null) targetPoint = receiver.transform.position;
                w.SetReceiverPosition(targetPoint);
            }
        }
        else if (Input.GetMouseButtonUp(2))
        {
            if (sender != null)
            {
                receiver = pickAtCursor();
                if (receiver != null) sender.AddReceiverDynamic(receiver.transform.position, receiver);
                if (w != null)
                    Destroy(w.gameObject);
            }
        }
        else
        {
            if (w != null)
                Destroy(w.gameObject);
        }


        if (Input.GetKeyDown(KeyCode.R)) builderCursor.RotateRight();
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BuilderCursor.builderCursor.RotateLeft();
        }
        */
    }

    private void Place()
    {
        var t = builderCursor.Get();
        if (t.legal)
            LevelManager.currentLevel.Add(t.levelObjectData, t.pos, t.rot);
    }

    private void Displace()
    {
        var o = builderCursor.GetGameObjectAt();
        LevelManager.currentLevel.Remove(o);
    }

    private void UpdatePosition()
    {
        // lock if control pressed
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            var targetDirection = transform.forward * Input.GetAxis("Vertical") +
                                  transform.right * Input.GetAxis("Horizontal") +
                                  transform.up * (Input.GetKey("space") ? 1 : 0) +
                                  -transform.up * (Input.GetKey("left shift") ? 1 : 0);
            transform.position += Time.deltaTime * Vector3.Normalize(targetDirection) * speed;
            builderPosition = transform.position;
        }
    }

    private void UpdateRotation()
    {
        var currentRotation = transform.localRotation.eulerAngles;
        currentRotation.y += Input.GetAxis("Mouse X");
        var oldX = currentRotation.x;
        currentRotation.x -= Input.GetAxis("Mouse Y");
        if (!(280 <= currentRotation.x && currentRotation.x <= 361 ||
              -1 <= currentRotation.x && currentRotation.x <= 80))
            currentRotation.x = oldX;
        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
    }
}