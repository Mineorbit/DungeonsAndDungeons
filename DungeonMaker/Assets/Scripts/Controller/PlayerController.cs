using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float Factor;
	float TargetSpeed = 0;
	public float BaseSpeed;
	public float Increase;
	public float Accelaration;
	public float Speed = 0;
	public float Gravity = 1;
	public Vector3 TargetDirection;
	Vector3 Direction;
	CharacterController Controller;
	public int Mask;
	public float Height = 0.75f;
	float runTime = 0;
	float errorThreshold = 0.01f;
	float SpeedY;
	public ModelController modelController;

	public float jumpForce;
	public bool inControl = false;
	public bool isGrounded = false;
	public bool Jumping = false;
	public bool Strike =  false;
	public float useTime = 0.5f;
	public Item left;
	public Item right;
	void Start () {
		Mask = LayerMask.GetMask ("Floor");
		Controller = GetComponent<CharacterController> ();
		modelController = GetComponentInChildren<ModelController>();
		modelController.left = left;
		modelController.right = right;
		TargetDirection = new Vector3(0,0,0);
	}

	void Update () {
		Strike = false;
		isGrounded = FloorCheck();
		//walken
		if(inControl)
		{
		if(Input.GetKey(KeyCode.LeftShift)&&isGrounded) TargetSpeed = BaseSpeed+Increase; else TargetSpeed = BaseSpeed;
		TargetDirection.x = Input.GetAxis("Vertical");
		TargetDirection.z = -Input.GetAxis("Horizontal");
		if(TargetDirection.x == 0 && TargetDirection.z == 0) TargetSpeed =  0;
		computeSpeed();
		}else
		{
			Speed = 0;
		}
		Direction = Speed *Vector3.Normalize(TargetDirection)*Time.deltaTime;
		
		Factor = Speed/(BaseSpeed+Increase);
		
		//fallen
		if(!isGrounded)
		{
			SpeedY+=-Gravity+Time.deltaTime;
		}else
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				Jumping = true;
				SpeedY = jumpForce;
			}else
			{
				Jumping = false;
				SpeedY = 0;
			}
		}
		Direction.y = SpeedY;
		Controller.Move (Direction);
		//Combat
		//Right Action
		if(Input.GetMouseButtonDown(0)&&!Jumping)
		{
			RightUse();
		}
		if(Input.GetMouseButtonDown(1)&&!Jumping)
		{
			LeftUse();
		}
		updateAnimator();
	}
	void updateAnimator()
	{
		modelController.isGrounded = isGrounded;
		modelController.Jumping = Jumping;
		modelController.Factor = Factor;
		modelController.TargetDirection = TargetDirection;
		modelController.inControl = inControl;
	}

	void computeSpeed()
	{
		if(TargetDirection.x == 0 && TargetDirection.z == 0)
		{
			if(Mathf.Abs(Speed)<errorThreshold)
			{
				Speed = 0;
			}else
			{
			Speed += 0.5f*Mathf.Sign(-Speed)*Accelaration*Time.deltaTime;
			}
		}else
		if(Input.GetKey(KeyCode.LeftShift))
		{
			if(Mathf.Abs((BaseSpeed+Increase)-Speed)<errorThreshold)
			{
				Speed = BaseSpeed+Increase;
			}else
			{
			Speed += 0.5f*Mathf.Sign(TargetSpeed-Speed)*Accelaration*Time.deltaTime;
			}
		}else{
			if(Mathf.Abs((BaseSpeed)-Speed)<errorThreshold)
			{
				Speed = BaseSpeed;
			}else
			{
			Speed += 0.5f*Mathf.Sign(TargetSpeed-Speed)*Accelaration*Time.deltaTime;
			}

		}
	}

	Vector3 pushBack (Vector3 direction, float force) {
		inControl = false;
		return force * Vector3.Normalize (direction);
	}
	void LeftUse()
	{
		useTime = left.useTime;
		setupUse();
		modelController.useL();
	}
	void RightUse()
	{
		useTime = right.useTime;
		setupUse();
		modelController.useR();
	}
	void setupUse()
	{
		Strike =  true;
		inControl = false;
		StartCoroutine("useItem");
	}
	bool FloorCheck () {
		Vector3 down = transform.up * -1;
		return Physics.Raycast (transform.position, down, Height, Mask);
	}

	void postUse()
	{
		left.animateDefault();
		right.animateDefault();
		Strike = false;
		inControl = true;
	}
	IEnumerator useItem()
    {
        yield return new WaitForSeconds(useTime);
		postUse();
    }

}