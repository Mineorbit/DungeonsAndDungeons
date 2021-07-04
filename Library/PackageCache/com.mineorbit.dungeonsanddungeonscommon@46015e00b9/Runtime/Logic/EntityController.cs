using System.Collections.Generic;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class EntityController : MonoBehaviour
    {
        private bool dirty;
        
        
        public float currentSpeed;


        private Vector3 lastPosition;
        
        private readonly int k = 10;
        
        public List<float> lastSpeeds = new List<float>();

        public virtual void Start()
        {
            
            lastPosition = transform.position;
        }

        public virtual void Update()
        {
            if (dirty)
            {
                dirty = false;
            }
        }

        public void OnSpawn(Vector3 location)
        {
        }

        public virtual void FixedUpdate()
        {
            ComputeCurrentSpeed();
        }

        private void ComputeCurrentSpeed()
        {
            lastSpeeds.Add((transform.position - lastPosition).magnitude / Time.deltaTime);
            if (lastSpeeds.Count > k) lastSpeeds.RemoveAt(0);
            float sum = 0;
            for (var i = 0; i < lastSpeeds.Count; i++) sum += lastSpeeds[i];
            currentSpeed = sum / k;
            lastPosition = transform.position;
        }
        
        
    }
}