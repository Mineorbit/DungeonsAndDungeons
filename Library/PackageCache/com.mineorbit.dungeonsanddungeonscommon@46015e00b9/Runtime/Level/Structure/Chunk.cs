using System;
using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Chunk : MonoBehaviour
    {
        public long chunkId;
        private readonly float eps = 0.05f;

        public bool finishedLoading = false;
        
        private void Start()
        {
            gameObject.name = "Chunk " + chunkId;
            Debug.Log("Chunk "+chunkId+" created");
        }

        public void OnDestroy()
        {
            Debug.Log("Chunk "+chunkId+" removed");
        }

        public LevelObject GetLevelObjectAt(Vector3 position)
        {
            Debug.Log("Searching for LevelObejct at "+position);
            foreach (LevelObject child in transform.GetComponentsInChildren<LevelObject>(includeInactive: true))
            {
                Debug.Log("Now at "+child);
                if ((child.transform.position - position).magnitude < eps)
                {
                    Debug.Log("Found "+child);
                    return child;
                }
                
            }
            return null;
        }
    }
}