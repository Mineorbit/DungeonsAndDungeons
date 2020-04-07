using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
	public Animator animator;
	public GameObject model;
	Quaternion targetRotation = Quaternion.identity;
	public Item left;
	public Item right;
	public bool isGrounded;
	public bool inControl;
	public bool Jumping;
	public float Factor;
	public Vector3 TargetDirection;

   // Start is called before the first frame update
    void Start()
    {
		animator = GetComponentInChildren<Animator>();
		model = animator.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		setAnimationValues();
        rotate();
		if(isGrounded)
		{
			postResetPosition();
		}
    }
	void setAnimationValues()
	{
	animator.SetFloat("Speed",Factor);
	animator.SetBool("OnGround",isGrounded);
	animator.SetBool("Jump",Jumping);
	}
	public void useR()
	{
		right.animateAction();
		animator.SetTrigger("Slash");
	}
	public void useL()
	{
		left.animateAction();
		animator.SetTrigger("Block");
	}
	public void postResetPosition()
	{
		model.transform.localPosition = new Vector3(0,0,0);
	}
    void rotate()
	{
	Vector3 targetDirection = TargetDirection;
	float swapz = targetDirection.z;
	targetDirection.z = -targetDirection.x;
	targetDirection.x = swapz;	
	targetDirection.y = 0;
 	targetRotation = transform.rotation;

	if(Input.GetAxis("Vertical")!=0||Input.GetAxis("Horizontal")!=0)	
	{	
	targetRotation.SetLookRotation(targetDirection,transform.up);
	}
	if(inControl)
	transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,0.25f);
	}
}
