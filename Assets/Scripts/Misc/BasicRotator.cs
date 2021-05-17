using UnityEngine;

public class BasicRotator : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0.25f, 0.5f, 1, 0);
    }
}