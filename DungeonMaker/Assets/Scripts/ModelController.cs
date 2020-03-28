using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
	public Animator animator;
 	public PlayerController controller;
	Quaternion targetRotation = Quaternion.identity;
 
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

	animator.SetFloat("Speed",controller.factor);
        rotate();
    }
    void rotate()
	{

	Vector3 targetDirection = controller.currentVelocity;
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
