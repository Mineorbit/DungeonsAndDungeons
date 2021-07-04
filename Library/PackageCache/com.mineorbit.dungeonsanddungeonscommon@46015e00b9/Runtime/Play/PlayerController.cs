using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class PlayerController : EntityController
    {
        public PlayerBaseAnimator playerBaseAnimator;
        public CharacterController controller;
        public Transform cam;
        public float Speed = 3f;
        public Vector3 targetDirection;
        public Vector3 movingDirection;
        public float speedY;


        public bool locallyControllable;
        public bool activated = true;
        public bool IsGrounded;
        public bool takeInput = true;
        public bool doInput;
        public bool isMe;
        public bool allowedToMove = true;

        public float heightRay = 0.55f;




        //Needs to be refined

        private Vector3 forwardDirection;
        private readonly float gravity = 4f;


        public Hitbox climbableHitbox;


        public bool inClimbing;
        
        //public static PlayerController currentPlayer;
        private Player player;

        private float turnSmoothVel;

        //Setup References for PlayerController and initial values if necessary
        public void Awake()
        {
            if (Camera.main != null)
                cam = Camera.main.transform;


            controller = transform.GetComponent<CharacterController>();
            climbableHitbox.Attach("Climbable");
            // POSSIBLE PROBLEM WITH INTERSECTIONS
            climbableHitbox.enterEvent.AddListener((x) =>
            {
                inClimbing = true;
            });
            climbableHitbox.exitEvent.AddListener((x) =>
            {
                if(climbableHitbox.insideCounter == 0)
                    inClimbing = false;
            });
            SetParams();
        }

        public override void Start()
        {
            base.Start();
            player = transform.GetComponent<Player>();
        }


        public override void Update()
        {
            UpdateGround();
            StateUpdate();

            playerBaseAnimator.speed = currentSpeed / 3;
            Move();
            base.Update();
        }

        

        private void SetParams()
        {
            heightRay = 1.1f;
        }

        public void UpdateGround()
        {
            var mask = 1 << 10;
            var hit = new RaycastHit();
            IsGrounded = controller.isGrounded || Physics.Raycast(transform.position, -Vector3.up, out hit, heightRay,
                mask, QueryTriggerInteraction.Ignore);
        }

        public float climbDampening = 0.5f;

        public float climbingSpeed = 1f;
        public void Move()
        {
            if (!IsGrounded && activated)
            {
                if(inClimbing)
                {
                    speedY = -gravity * climbDampening;
                }
                else
                {
                    speedY -= gravity * Time.deltaTime;
                }
            }
            if (IsGrounded || !activated) speedY = 0;
            targetDirection = new Vector3(0, 0, 0);

            if (doInput && takeInput)
            {
                if (cam != null)
                    targetDirection =
                        Vector3.Normalize(
                            Vector3.ProjectOnPlane(cam.right, transform.up) * Input.GetAxisRaw("Horizontal") +
                            Vector3.ProjectOnPlane(cam.forward, transform.up) * Input.GetAxisRaw("Vertical"));


                // Pickup closest item
                if (Input.GetKeyDown(KeyCode.G)) player.Invoke(player.UpdateEquipItem);

                if (Input.GetKeyDown(KeyCode.G))
                {
                    // INVOKE
                    // INTERACT WITH ENVIRONMENT (READ TEXT, PRESS BUTTON etc)
                }

                // change to angle towards ladder later on
                if (targetDirection.sqrMagnitude >= 0.01f && inClimbing)
                {
                    speedY = climbingSpeed;
                }
                
                if (IsGrounded)
                    if (Input.GetKeyDown(KeyCode.Space))
                        speedY = 1.5f;
                if (Input.GetMouseButton(0))
                    //INVOKE
                    player.Invoke(player.UseLeft, true);
                else if (Input.GetMouseButton(1))
                    //INVOKE
                    player.Invoke(player.UseRight, true);
                if (Input.GetMouseButtonUp(1)) player.Invoke(player.StopUseRight, true);
            }

            if (activated)
            {
                targetDirection.y = speedY;
                if (targetDirection.sqrMagnitude >= 0.01f)
                    movingDirection = targetDirection;
                else
                    movingDirection = new Vector3(0, 0, 0);
                controller.Move(targetDirection * Speed * Time.deltaTime);
            
                if (currentSpeed > 0) forwardDirection = (forwardDirection + movingDirection) / 2;
                if (doInput && takeInput)
                {
                    var angleY = 180 + 180 / Mathf.PI * Mathf.Atan2(forwardDirection.x, forwardDirection.z);
                    transform.eulerAngles = new Vector3(0, angleY, 0);
                }
            }
        }

        private void StateUpdate()
        {
            doInput = PlayerManager.acceptInput && allowedToMove && activated &&
                      locallyControllable; //&& !player.lockNetUpdate;
        }
    }
}