using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
	public Animator animator;
	public GameObject model;
 	public PlayerController controller;
	Quaternion targetRotation = Quaternion.identity;
 
   // Start is called before the first frame update
    void Start()
    {
        controller.modelController = this;
		model = animator.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		setAnimationValues();
        rotate();
    }
	void setAnimationValues()
	{
	animator.SetFloat("Speed",controller.Factor);
	animator.SetBool("OnGround",controller.isGrounded);
	animator.SetBool("Jump",controller.Jumping);
	}
	public void strikeR()
	{
		animator.SetTrigger("Slash");
	}
	public void strikeL()
	{
		animator.SetTrigger("Block");
	}
	public void postStrike()
	{
		model.transform.localPosition = new Vector3(0,0,0);
	}
    void rotate()
	{

	Vector3 targetDirection = controller.TargetDirection;
	float swapz = targetDirection.z;
	targetDirection.z = -targetDirection.x;
	targetDirection.x = swapz;	
	targetDirection.y = 0;
 	targetRotation = transform.rotation;

	if(Input.GetAxis("Vertical")!=0||Input.GetAxis("Horizontal")!=0)	
	{	
	targetRotation.SetLookRotation(targetDirection,transform.up);
	}
	if(controller.inControl)
	transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,0.25f);
	
}
}
