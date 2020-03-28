using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

	Renderer modelRenderer;
	Renderer baseRenderer;
	MeshFilter   model;
	Material invalid;
	Material valid;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Model = GameObject.Find("Model");
	GameObject Base  = GameObject.Find("Base");
	modelRenderer = Model.GetComponent<Renderer>();
	baseRenderer = Base.GetComponent<Renderer>();
	model = Model.GetComponent<MeshFilter>();
	valid = Resources.Load<Material>("Mat/CursorValid");
	invalid = Resources.Load<Material>("Mat/Cursor");

		modelRenderer.material = invalid;
		baseRenderer.material = invalid;
    }

   
	public void setCursor(bool ValidPosition, Mesh CursorMesh,PreviewData p)
	{
		model.mesh = CursorMesh;
		model.transform.localScale = p.previewScale;
		model.transform.rotation = p.previewRotation;
		modelRenderer.material = ValidPosition?valid:invalid;
		baseRenderer.material = ValidPosition?valid:invalid;
		
	}

	
}
