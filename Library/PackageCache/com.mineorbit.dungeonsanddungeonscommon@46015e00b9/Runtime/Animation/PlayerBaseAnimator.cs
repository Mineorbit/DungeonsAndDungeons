using UnityEngine;
using UnityEngine.Serialization;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class PlayerBaseAnimator : EntityBaseAnimator
    {
        public CharacterController characterController;
        public GameObject footFX;
        private Vector3 forwardDirection;

        private Vector3 oldAngles;

        public void Start()
        {
            forwardDirection = new Vector3(0, 0, 0);
        }


        public PlayerController GetController()
        {
            return (PlayerController) entityController;
        }
        
        public override void Update()
        {
            base.Update();
            //simplify in future to just moving somehow

            if (me == PlayerManager.currentPlayer)
                footFX.SetActive(GetController().IsGrounded && GetController().allowedToMove &&
                                 (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                                  Input.GetKey(KeyCode.D)));
        }

        public void ChangeitemSetting(int leftSide)
        {


            // WE NEED TO DETERMINE ITEM TYPE FROM USED IN FUTURE

            if (leftSide == 0)
            {
                oldAngles = ((Player) me).GetLeftHandle().slot.transform.localEulerAngles;

                ((Player) me).GetLeftHandle().slot.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                oldAngles = ((Player) me).GetRightHandle().transform.localEulerAngles;


                ((Player) me).GetRightHandle().slot.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        public void ChangeItemSettingBack(int leftSide)
        {
            if (leftSide == 0)
                ((Player) me).GetLeftHandle().slot.transform.localEulerAngles = oldAngles;
            else
                ((Player) me).GetRightHandle().slot.transform.localEulerAngles = oldAngles;
        }
    }
}