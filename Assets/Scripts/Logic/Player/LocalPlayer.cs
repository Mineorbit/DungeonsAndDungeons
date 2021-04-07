using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    public override void Start()
    {
        base.Start();
        SetupPlay();
    }
    void SetupPlay()
    {
        gameObject.name = "Player" + localId;
    }
    void Update()
    {
        if (transform.position.y < -8)
            Kill();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.collider.gameObject;
        if (col.tag == "Enemy")
        {
            //int damage = col.GetComponent<EnemyController>().damage;
            int damage = 10;
            if (damage>0)
            {
                Hit(damage);
            }
        }
    }
}
