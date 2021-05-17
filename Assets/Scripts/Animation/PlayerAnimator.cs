using com.mineorbit.dungeonsanddungeonscommon;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator characterAnimator;
    public ParticleSystem[] runDust;

    public PlayerAnimationEventHandler playerAnimationEventHandler;
    private CharacterController controller;
    private PlayerController playerController;
    private bool playingDust;


    private void Start()
    {
        playerAnimationEventHandler = transform.GetComponentInChildren<PlayerAnimationEventHandler>();


        controller = transform.GetComponent<CharacterController>();
        playerController = transform.GetComponent<PlayerController>();
        characterAnimator = transform.Find("character").GetComponent<Animator>();
        runDust = transform.Find("Particles").Find("Running").GetComponentsInChildren<ParticleSystem>();
        playingDust = true;
        StopDust();
    }

    private void Update()
    {
        var targetDirection = playerController.movingDirection;
        targetDirection.y = 0;
        var inputVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        var movementSpeed = playerController.currentSpeed;
        characterAnimator.SetFloat("Speed", movementSpeed);
        var movementKeyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                                 Input.GetKey(KeyCode.D);


        if (movementKeyPressed && playerController.doInput)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.2f);


        if (movementSpeed > 0 && playerController.IsGrounded)
            StartDust();
        else
            StopDust();
    }


    public void Attack()
    {
        playerAnimationEventHandler.Attack();
        characterAnimator.SetTrigger("Attack");
    }


    private void StartDust()
    {
        if (!playingDust)
        {
            playingDust = true;
            runDust[0].Play();
            runDust[1].Play();
        }
    }

    private void StopDust()
    {
        if (playingDust)
        {
            playingDust = false;
            runDust[0].Stop();
            runDust[1].Stop();
        }
    }
}