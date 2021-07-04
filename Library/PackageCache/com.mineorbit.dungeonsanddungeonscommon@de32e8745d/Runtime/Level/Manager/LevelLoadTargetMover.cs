using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class LevelLoadTargetMover : MonoBehaviour
    {
        public bool follow = true;
        public Transform target;
        public void Update()
        {
            if (follow && target != null) transform.position = target.position;
        }
    }
}