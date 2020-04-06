using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewData : MonoBehaviour {
    public Mesh previewMesh;
    public Vector3 previewScale;
    public Vector3 previewRescale;
    public Vector3 modelDisplacement;
    public Quaternion previewRotation;
    public Quaternion previewReRotation;
    public GameManager.Selectable thisElement;
    public void apply () {
        transform.rotation = previewReRotation;
        transform.localScale = Vector3.Scale (previewScale, previewRescale);
    }
}