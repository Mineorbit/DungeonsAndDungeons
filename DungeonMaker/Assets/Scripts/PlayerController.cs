using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float factor;
	public float Speed;
	public float TargetSpeed;
	public float BaseSpeed = 2;
	public float Increase = 2;
	float Gravity = 0.1f;

	public float TargetGravity = 0.1f;
	public float speedY = 0;
	Vector3 direction;
	public bool onGround = false;
	public float height = 0.5f;
	public Vector3 currentVelocity;
	CharacterController controller;
	public bool Host = true;
	public bool inControl = true;
	public bool Hit = false;
	float HitForce = 0;
	public float jumpForce = 10;
	public bool jumping;
	Vector3 HitDirection = new Vector3 (0, 0, 0);

	public int mask;
	void Start () {
		mask = LayerMask.GetMask ("Floor");
		controller = GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift)) {
			TargetSpeed = BaseSpeed + Increase;
		} else TargetSpeed = BaseSpeed;
		onGround = FloorCheck ();

		Vector3 targetDirection = new Vector3 (0, 0, 0);

		if (Hit) {
			targetDirection = pushBack ((-1 * controller.velocity + HitDirection) / 2, HitForce);
			Hit = false;
			HitForce = 0;
			HitDirection = new Vector3 (0, 0, 0);
		}

		if (onGround && jumping) jumping = false;

		Debug.DrawRay (transform.position, transform.up * -1, Color.red, height, true);

		if (inControl) {
			targetDirection = Input.GetAxis ("Horizontal") * -1 * transform.forward + Input.GetAxis ("Vertical") * transform.right;
			targetDirection = Vector3.Normalize (targetDirection);
		}

		Speed = Mathf.Lerp (Speed, TargetSpeed, 0.5f);

		Gravity = Mathf.Lerp (Gravity, TargetGravity, 0.5f);

		targetDirection *= Speed;

		Vector3 vel = controller.velocity;
		vel.y = 0;
		factor = (vel.magnitude) / BaseSpeed;
		if (factor == 0 && !inControl) {
			inControl = true;
		}

		direction = Vector3.Lerp (direction, targetDirection, 0.5f);

		speedY = onGround ? 0 : speedY + Gravity;

		if (Input.GetKeyDown (KeyCode.Space) && !jumping) {
			Debug.Log ("Jump");
			jump ();
		}

		direction.y = -speedY;

		if (Input.GetKeyDown (KeyCode.F)) takeDamage (10, -1 * controller.velocity);
		move ();

	}

	Vector3 pushBack (Vector3 direction, float force) {
		inControl = false;
		return force * Vector3.Normalize (direction);
	}

	public void takeDamage (float hitForce, Vector3 directionFromForce) {
		Hit = true;
		HitForce = hitForce;
		HitDirection = directionFromForce;
	}

	void move () {
		controller.Move (direction);
		currentVelocity = controller.velocity;
	}
	void jump () {
		jumping = true;
		speedY -= jumpForce;
	}

	bool FloorCheck () {
		Vector3 down = transform.up * -1;
		return Physics.Raycast (transform.position, down, height, mask);
	}
}