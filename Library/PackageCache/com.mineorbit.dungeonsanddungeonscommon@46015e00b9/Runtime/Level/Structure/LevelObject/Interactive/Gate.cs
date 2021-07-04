using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Gate : InteractiveLevelObject
    {
        public Collider gateCollider;
        public GateBaseAnimator gateBaseAnimator;
        public NavMeshObstacle navMeshObstacle;

        public override void OnInit()
        {
            base.OnInit();
            gateBaseAnimator.Close();
            gateCollider.enabled = true;
            navMeshObstacle.enabled = true;
        }

        public override void Activate()
        {
            base.Activate();
            SetOpen();
            navMeshObstacle.enabled = false;
        }

        public void SetOpen()
        {
            Debug.Log("Opening Gate");
            gateBaseAnimator.Open();
            gateCollider.enabled = false;
        }

        public void SetClosed()
        {
            Debug.Log("Closing Gate");
            gateBaseAnimator.Close();
            gateCollider.enabled = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            SetClosed();
            navMeshObstacle.enabled = true;
        }
    }
}