using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    Vector2 lastPosition;
    Vector2 lastObjectPosition;
    Vector3 targetPosition;
    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        updatePosition();
    }
    void updatePosition()
    {
        Vector2 offset;
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            lastObjectPosition = transform.position;
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            offset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lastPosition;
            targetPosition = new Vector3(lastObjectPosition.x + offset.x, lastObjectPosition.y + offset.y, 0);
        }
        transform.position = (transform.position + targetPosition) / 2;
    }
}
