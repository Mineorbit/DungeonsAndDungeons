using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class BuilderCursor : MonoBehaviour
{
    private static float degree;
    public static BuilderCursor builderCursor;

    private static LevelObjectData currentSelection;


    private static Vector3 normalVec;

    private static MeshFilter cursorMesh;
    private static MeshRenderer cursorRender;


    private static GameObject hitObject;
    private static Transform cursorModel;

    private static bool placementLegal;
    private static Vector3 offset;

    public static Vector3 targetPosition;

    public Material legalMat;
    public Material illlegalMat;

    public int direction;

    public Mesh defaultMesh;
    private readonly float maxDistance = 200;

    private void Update()
    {
        ComputeCursorPosition();
        UpdateMaterial();
    }

    public void OnEnable()
    {
        if (builderCursor != null) Destroy(this);
        builderCursor = this;
        cursorModel = transform.Find("model");
        cursorMesh = cursorModel.GetComponent<MeshFilter>();
        cursorRender = cursorModel.GetComponent<MeshRenderer>();

        degree = 1f;
    }

    public void OnDisable()
    {
        builderCursor = null;
    }

    public void SetLocation(Vector3 target, Vector3 normal)
    {
        float g = 1;
        var position = LevelManager.GetGridPosition(target + currentSelection.granularity*normal, currentSelection);
        normalVec = normal;

        UpdateCursor(position);
    }

    public static void Set(LevelObjectData objectType)
    {
        currentSelection = objectType;
        UpdateMesh();
    }


    public void RotateLeft()
    {
        direction = (direction - 1) % 4;
    }

    public void RotateRight()
    {
        direction = (direction + 1) % 4;
    }

    private void UpdateMaterial()
    {
        var newLegal = UpdateLegal();
        if (newLegal != placementLegal)
        {
            placementLegal = newLegal;
            if (placementLegal)
                cursorRender.material = builderCursor.legalMat;
            else
                cursorRender.material = builderCursor.illlegalMat;
        }
    }

    private bool UpdateLegal()
    {
        var layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        var dist = 0.5f;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f * dist * Vector3.forward, Vector3.forward, out hit,
            dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.forward, -Vector3.forward, out hit,
            dist, layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f * dist * Vector3.up, Vector3.up, out hit, dist,
            layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.up, -Vector3.up, out hit, dist,
            layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position - 0.5f * dist * Vector3.right, Vector3.right, out hit, dist,
            layerMask)) return false;
        if (Physics.Raycast(cursorModel.transform.position + 0.5f * dist * Vector3.right, -Vector3.right, out hit, dist,
            layerMask)) return false;
        return true;
    }

    private void ComputeCursorPosition()
    {
        var layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 targetLocation;

        var start = Camera.main.transform.position;
        var forward = Camera.main.transform.forward;

        if (Physics.Raycast(start, forward, out hit, Mathf.Infinity, layerMask))
        {
            targetPosition = hit.point;
            targetLocation = start + forward * hit.distance;
            hitObject = hit.transform.gameObject;
        }
        else
        {
            targetLocation = start + forward * maxDistance;
        }

        SetLocation(targetLocation, hit.normal);
    }

    private void UpdateCursor(Vector3 position)
    {
        builderCursor.transform.position = position;

        var off = new Vector3(0, 0, 0);
        var rot = new Vector3(0, 0, 0);
        var scal = new Vector3(1, 1, 1);
        if (currentSelection != null)
        {
            off = currentSelection.cursorOffset;
            rot = currentSelection.cursorRotation;
            scal = currentSelection.cursorScale;
        }

        cursorModel.transform.localEulerAngles = rot;
        cursorModel.transform.localScale = scal;
        cursorModel.transform.position = position + cursorModel.transform.TransformVector(off);
        builderCursor.transform.localEulerAngles = new Vector3(0, builderCursor.direction * 90f, 0);
    }

    private static void UpdateMesh()
    {
        Mesh targetMesh = null;
        if (currentSelection != null)
        {
            targetMesh = currentSelection.GetMesh();
            targetMesh.SetTriangles(targetMesh.triangles, 0);
        }
        else
        {
            targetMesh = builderCursor.defaultMesh;
        }

        if (targetMesh != null) cursorMesh.mesh = targetMesh;
    }

    public LevelObject GetLevelObjectAt()
    {
        var o = hitObject.GetComponent<LevelObject>();
        if (o != null) return o;
        return null;
    }

    public GameObject GetGameObjectAt()
    {
        var o = hitObject;
        if (o != null) return o;
        return null;
    }

    public (Vector3 pos, Quaternion rot, Vector3 norm, LevelObjectData levelObjectData, bool legal) Get()
    {
        return (cursorModel.transform.position, builderCursor.transform.rotation, normalVec, currentSelection,
            placementLegal);
    }
}