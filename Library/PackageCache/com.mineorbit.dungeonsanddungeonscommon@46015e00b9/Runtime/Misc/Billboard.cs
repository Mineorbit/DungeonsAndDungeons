using UnityEngine;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class Billboard : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}