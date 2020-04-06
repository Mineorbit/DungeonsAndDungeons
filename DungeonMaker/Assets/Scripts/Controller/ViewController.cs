using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {
	public Camera camera;
	public Vector3 target;
	public float gridGranularity = 5;
	GameManager gameManager;
	// Start is called before the first frame update
	void Start () {
		camera = GetComponentInChildren<Camera> ();
		target = new Vector3 (0, 0, 0);
		gameManager =  GameObject.Find("GameManager").transform.GetComponent<GameManager>();
		
	}

	Vector3 getClose (Vector3 point) {
		float ofx = Mathf.Repeat (point.x, gridGranularity);
		float ofy = Mathf.Repeat (point.y, gridGranularity);
		float ofz = Mathf.Repeat (point.z, gridGranularity);
		float x = ofx > gridGranularity / 2 ? point.x - ofx + gridGranularity : point.x - ofx;
		float y = ofy > gridGranularity / 2 ? point.y - ofy + gridGranularity : point.y - ofy;
		float z = ofz > gridGranularity / 2 ? point.z - ofz + gridGranularity : point.z - ofz;
		return new Vector3 (x, y, z);
	}
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Input.GetAxis ("Vertical") - transform.forward * Input.GetAxis ("Horizontal");
		findTarget ();

		if (Input.GetMouseButton (2)) rotate ();
	}
	void findTarget() {
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			Vector3 hitPoint = hit.point;
			target = getClose (hitPoint) + new Vector3 (0, gridGranularity, 0);
			gameManager.cursorLocation = target;
		}
		gameManager.updateCursor();
	}
	void rotate () {
		transform.Rotate (0f, Input.GetAxis ("Mouse X"), 0f);
	}
}