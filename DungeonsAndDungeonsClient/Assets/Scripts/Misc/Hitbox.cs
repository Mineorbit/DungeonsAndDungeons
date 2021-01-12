using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Hitbox : MonoBehaviour
{

    public UnityEvent<GameObject> enterEvent = new UnityEvent<GameObject>();
    Collider collider;

    string targetTag;

    void Start()
    {
    }

    public void Attach(GameObject p,string target,Vector3 offset)
    {
        targetTag = target;
        transform.parent = p.transform;
        transform.localPosition = offset;
        collider = GetComponent<Collider>();
    }

    public void Activate()
    {
    collider.enabled = true;
    }

    public void Deactivate()
    {
    collider.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == targetTag)
        {
            enterEvent.Invoke(other.gameObject);
        }
    }
}
